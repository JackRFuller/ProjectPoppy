[System.Serializable]
public class LocalisationData
{
    public LocalisationItem[] items;	
}

[System.Serializable]
public class LocalisationItem
{
    public LocalisationKeys.StringKeys keys;
    public string value;
}

[System.Serializable]
public class LocalisationKeys
{
    public enum StringKeys
    {
        Key1,
        Key2,
        Key3,
    }
}



