package md58259199fa797afdc4d4da4974cb449e6;


public class AzurePushBroadcastReceiver
	extends md5214eafb7e7b3b7fcc363a68a6358563f.GcmBroadcastReceiverBase_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("App8.AzurePushBroadcastReceiver, App9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AzurePushBroadcastReceiver.class, __md_methods);
	}


	public AzurePushBroadcastReceiver ()
	{
		super ();
		if (getClass () == AzurePushBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("App8.AzurePushBroadcastReceiver, App9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
