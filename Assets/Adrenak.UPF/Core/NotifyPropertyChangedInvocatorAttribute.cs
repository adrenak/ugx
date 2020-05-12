using System;

namespace Adrenak.UPF {
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute {
        public NotifyPropertyChangedInvocatorAttribute() {
        }

        public NotifyPropertyChangedInvocatorAttribute(string parameterName) {
            ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }
}