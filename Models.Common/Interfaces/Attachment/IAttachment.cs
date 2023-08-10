using Helpers.File;

namespace Models.Common
{
    public interface IAttachment<T> where T : IFileModel, new()
    {
        public T Attachment { get; set; }
    }
}