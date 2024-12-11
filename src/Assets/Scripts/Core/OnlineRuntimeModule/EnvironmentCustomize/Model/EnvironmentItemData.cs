namespace Core.OnlineRuntimeModule.EnvironmentCustomize.Model
{
    public class EnvironmentItemData
    {
        public int ID = 0;
        public string UID;
        public float PosX = 0;
        public float PosY = 0;
        public float PosZ = 0;
        public float RotY = 0;

        public void Copy(EnvironmentItemData data)
        {
            PosX = data.PosX;
            PosY = data.PosY;
            PosZ = data.PosZ;
            RotY = data.RotY;
        }
    }
}