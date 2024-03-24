namespace Base.Data
{
    public interface IBaseData
    {
        public delegate void OnDataChange();
        public event OnDataChange onDataChange;
    }
}
