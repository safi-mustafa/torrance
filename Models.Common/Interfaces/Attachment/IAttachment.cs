using Helpers.File;

namespace Models.Common
{
    public interface IFCOLogAttachment<T> where T : IFileModel, new()
    {
        public T Photo { get; set; }
        public T File { get; set; }
    }
}