using System;

namespace Juick.Client.ViewModels {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ViewModelAttribute : Attribute {
        public string Key { get; private set; }
        public Type Type { get; private set; }

        public ViewModelAttribute(string key, Type type) {
            Key = key;
            Type = type;
        }
    }
}
