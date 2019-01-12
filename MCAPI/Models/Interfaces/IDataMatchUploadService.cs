using Mc.TD.Upload.Domain.DataMatch;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Mc.TD.Upload.Base.Interfaces.DataMatch
{
    public interface IDataMatchUploadService
    {
        Task<DataMatchUploadResponse> UploadDataMatchFile(DataMatchUploadRequestBody v, string str, string str2);
    }
}