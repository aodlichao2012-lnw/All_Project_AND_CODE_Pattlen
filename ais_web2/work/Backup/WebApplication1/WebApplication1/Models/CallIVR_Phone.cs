

using IVRCall_Dll_Agen;

public partial class CallIVR_Phone : IVRCall_Dll_Agen.IVRCall_Dll_Agen
{

    public event NewUpdateEventHandler NewUpdate;

    public delegate void NewUpdateEventHandler(string msg);

    public override void internalcallback(string str)
    {
        updateGUI(str.ToString());

    }

    private void updateGUI(string msg)
    {
        NewUpdate?.Invoke(msg);
    }

}