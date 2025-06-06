using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace UniHumanoid
{
    public class Bvh
    {
        public BvhNode Root
        {
            get;
            private set;
        }

        public TimeSpan FrameTime
        {
            get;
            private set;
        }

        public BvhChannelCurve[] Channels
        {
            get;
            private set;
        }

        int m_frames;
        public int FrameCount
        {
            get { return m_frames; }
        }

        public struct PathWithProperty
        {
            public string Path;
            public string Property;
            public bool IsLocation;
        }

        public bool TryGetPathWithPropertyFromChannel(BvhChannelCurve channel, out PathWithProperty pathWithProp)
        {
            var index = Channels.ToList().IndexOf(channel);
            if (index == -1)
            {
                pathWithProp = default(PathWithProperty);
                return false;
            }

            foreach (var node in Root.Traverse())
            {
                for (int i = 0; i < node.Channels.Length; ++i, --index)
                {
                    if (index == 0)
                    {
                        pathWithProp = new PathWithProperty
                        {
                            Path = GetPath(node),
                            Property = node.Channels[i].ToProperty(),
                            IsLocation = node.Channels[i].IsLocation(),
                        };
                        return true;
                    }
                }
            }

            throw new BvhException("channel is not found");
        }

        public string GetPath(BvhNode node)
        {
            var list = new List<string>() { node.Name };

            var current = node;
            while (current != null)
            {
                current = GetParent(current);
                if (current != null)
                {
                    list.Insert(0, current.Name);
                }
            }

            return String.Join("/", list.ToArray());
        }

        BvhNode GetParent(BvhNode node)
        {
            foreach (var x in Root.Traverse())
            {
                if (x.Children.Contains(node))
                {
                    return x;
                }
            }

            return null;
        }

        public BvhChannelCurve GetChannel(BvhNode target, BvhChannel channel)
        {
            var index = 0;
            foreach (var node in Root.Traverse())
            {
                for (int i = 0; i < node.Channels.Length; ++i, ++index)
                {
                    if (node == target && node.Channels[i] == channel)
                    {
                        return Channels[index];
                    }
                }
            }

            throw new BvhException("channel is not found");
        }

        public override string ToString()
        {
            return string.Format("{0}nodes, {1}channels, {2}frames, {3:0.00}seconds"
                , Root.Traverse().Count()
                , Channels.Length
                , m_frames
                , m_frames * FrameTime.TotalSeconds);
        }

        public Bvh(BvhNode root, int frames, float seconds)
        {
            Root = root;
            FrameTime = TimeSpan.FromSeconds(seconds);
            m_frames = frames;
            var channelCount = Root.Traverse()
                .Where(x => x.Channels != null)
                .Select(x => x.Channels.Length)
                .Sum();
            Channels = Enumerable.Range(0, channelCount)
                .Select(x => new BvhChannelCurve(frames))
                .ToArray()
                ;
        }

        public void ParseFrame(int frame, string line)
        {
            var split = line.Trim().Split().Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (split.Length != Channels.Length)
            {
                throw new BvhException("frame key count is not match channel count");
            }
            for (int i = 0; i < Channels.Length; ++i)
            {
                Channels[i].SetKey(frame, float.Parse(split[i], System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        public static Bvh Parse(string src)
        {
            using (var r = new StringReader(src))
            {
                var first = r.ReadLine();
                if (first != "HIERARCHY")
                {
                    throw new BvhException("not start with HIERARCHY");
                }

                var root = ParseNode(r);
                if (root == null)
                {
                    return null;
                }

                var frames = 0;
                var frameTime = 0.0f;
                if (r.ReadLine() == "MOTION")
                {
                    var frameSplit = r.ReadLine().Split(':');
                    if (frameSplit[0] != "Frames")
                    {
                        throw new BvhException("Frames is not found");
                    }
                    frames = int.Parse(frameSplit[1]);

                    var frameTimeSplit = r.ReadLine().Split(':');
                    if (frameTimeSplit[0] != "Frame Time")
                    {
                        throw new BvhException("Frame Time is not found");
                    }
                    frameTime = float.Parse(frameTimeSplit[1], System.Globalization.CultureInfo.InvariantCulture);
                }

                var bvh = new Bvh(root, frames, frameTime);

                for (int i = 0; i < frames; ++i)
                {
                    var line = r.ReadLine();
                    bvh.ParseFrame(i, line);
                }

                return bvh;
            }
        }

        static BvhNode ParseNode(StringReader r, int level = 0)
        {
            var firstline = r.ReadLine().Trim();
            var split = firstline.Split();
            if (split.Length != 2)
            {
                if (split.Length == 1)
                {
                    if (split[0] == "}")
                    {
                        return null;
                    }
                }
                throw new BvhException(String.Format("split to {0}({1})", split.Length, firstline));
            }

            BvhNode node = null;
            if (split[0] == "ROOT")
            {
                if (level != 0)
                {
                    throw new BvhException("nested ROOT");
                }
                node = new BvhNode(split[1]);
            }
            else if (split[0] == "JOINT")
            {
                if (level == 0)
                {
                    throw new BvhException("should ROOT, but JOINT");
                }
                node = new BvhNode(split[1]);
            }
            else if (split[0] == "End")
            {
                if (level == 0)
                {
                    throw new BvhException("End in level 0");
                }
                node = new BvhEndSite();
            }
            else
            {
                throw new BvhException("unknown type: " + split[0]);
            }

            if (r.ReadLine().Trim() != "{")
            {
                throw new BvhException("'{' is not found");
            }

            node.Parse(r);

            // child nodes
            while (true)
            {
                var child = ParseNode(r, level + 1);
                if (child == null)
                {
                    break;
                }

                if (!(child is BvhEndSite))
                {
                    node.Children.Add(child);
                }
            }

            return node;
        }
    }
}
