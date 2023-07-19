using API.Quereseres.Interfaces;

namespace API.Quereseres.Wrappers
{
    public class ComplexWrapper<T> : SimpleWrapper
    {
        public T? Result { get; set; }
    }
}
