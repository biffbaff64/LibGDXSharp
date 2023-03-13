namespace LibGDXSharp.Core
{
    /// <summary>
    /// A Preference instance is a hash map holding different values. It is stored
    /// in a "%USERPROFILE%/.prefs" directory. CAUTION: On the desktop platform,
    /// all libgdx applications share the same ".prefs" directory. To avoid collisions
    /// use specific names like "game1.settings" instead of "settings".
    /// To persist changes made to a preferences instance Flush() has to be invoked.
    /// </summary>
    public interface IPreferences
    {
        public IPreferences PutBoolean (string key, bool val);

        public IPreferences PutInteger (string key, int val);

        public IPreferences PutLong (string key, long val);

        public IPreferences PutFloat (string key, float val);

        public IPreferences PutString (string key, string val);

        public IPreferences Put (Dictionary<string, object> vals);

        public bool GetBoolean (string key);

        public int GetInteger (string key);

        public long GetLong (string key);

        public float GetFloat (string key);

        public string GetString (string key);

        public bool GetBoolean (string key, bool defValue);

        public int GetInteger (string key, int defValue);

        public long GetLong (string key, long defValue);

        public float GetFloat (string key, float defValue);

        public string GetString (string key, string defValue);

        public Dictionary<string, object> Get ();

        public bool Contains (string key);

        public void Clear ();

        public void Remove (string key);

        public void Flush ();
    }
}
