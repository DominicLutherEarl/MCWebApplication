using Mc.TD.Upload.Base.Interfaces.DataMatch;
using Mc.TD.Upload.Domain.DataMatch;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.Web.ModelBinding;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Mc.TD.Upload.Api.Controllers
{
    public class DataMatchUploadController : ApiController
    {
        public IDataMatchUploadService _dataMatchUploadResponse;
        public IDataMatchUploadService _dMUResponse;

        public DataMatchUploadController()
        {
        
        }

        public async void UploadToBLOB()
        {
            CloudStorageAccount _cloudStorageAccount = null;
            CloudBlobContainer _cloudBlobContainer = null;
            BlobContinuationToken _blobContinuationToken = null;
            CloudBlobClient _cloudBlobClient;
            //BlobContainerPermissions _permissions;
            //CloudBlockBlob _cloudBlockBlob;

            string sourceFile = null;
            string destinationFile = null;
            string storageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string localFileName = "QuickStart_" + Guid.NewGuid().ToString() + ".txt";
            sourceFile = Path.Combine(localPath, localFileName);

            File.WriteAllText(sourceFile, "Hello, World!");
            if (CloudStorageAccount.TryParse(storageConnectionString, out _cloudStorageAccount))
            {
                try
                {
                    _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
                    _cloudBlobContainer = _cloudBlobClient.GetContainerReference("quickstartblobs" + Guid.NewGuid().ToString());
                    await _cloudBlobContainer.CreateAsync();
                    await _cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                    await _cloudBlobContainer.GetBlockBlobReference(localFileName).UploadFromFileAsync(sourceFile);
                    do
                    {
                        var results = await _cloudBlobContainer.ListBlobsSegmentedAsync(null, _blobContinuationToken);
                        _blobContinuationToken = results.ContinuationToken;
                        foreach (IListBlobItem item in results.Results)
                        {
                            Console.WriteLine(item.Uri);
                        }
                    } while (_blobContinuationToken != null);
                    destinationFile = sourceFile.Replace(".txt", "_DOWNLOADED.txt");
                    await _cloudBlobContainer.GetBlockBlobReference(localFileName).DownloadToFileAsync(destinationFile, FileMode.Create);
                }
                catch (StorageException ex)
                {
                }
                finally
                {
                    if (_cloudBlobContainer != null)
                    {
                        await _cloudBlobContainer.DeleteIfExistsAsync();
                    }
                    File.Delete(sourceFile);
                    File.Delete(destinationFile);
                }
            }
        }

        public DataMatchUploadController(IDataMatchUploadService dataMatchUploadResponse, DMUResponse dMUResponse)
        {
            _dataMatchUploadResponse = dataMatchUploadResponse;
            //_dMUResponse = dMUResponse;
        }

        [HttpPost]
        [Route("1")]
        [ResponseType(typeof(DMUResponse))]
        public async Task<DMUResponse> PostMatchedDataFiles([FromBody] DataMatchUploadRequestBody UploadedFile)
        {
            Validate(UploadedFile);
            DMUResponse _dMUResponse = new DMUResponse();
            if (!ModelState.IsValid)
            {
                _dMUResponse.orderId = (UploadedFile.requestheader.orderid == null) ? string.Empty : UploadedFile.requestheader.orderid;
                _dMUResponse.errorData = new List<ErrorData>();
                foreach (var field in ModelState.Keys)
                {
                    if (ModelState[field].Errors != null)
                    {
                        foreach (var _error in ModelState[field].Errors)
                        {
                            ErrorData _errorData = new ErrorData()
                            {
                                errorCause = "Invalid Request",
                                errorField = field,
                                errorExplanation = _error.ErrorMessage,
                                errorValidationType = ""
                            };
                            _dMUResponse.errorData.Add(_errorData);
                        }
                    }
                }
                return _dMUResponse;
            }
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string businessId = string.Empty;
            string fileId = string.Empty;
            if (headers.Contains("businessId"))
            {
                businessId = headers.GetValues("businessId").FirstOrDefault();
            }
            if (headers.Contains("businessId"))
            {
                fileId = headers.GetValues("fileId").FirstOrDefault();
            }
            return new DMUResponse();
            //return GenerateResponse(await _dataMatchUploadResponse.UploadDataMatchFile(UploadedFile, businessId, fileId));
        }


        [HttpPost]
        [Route("2")]
        [ResponseType(typeof(DataMatchUploadResponseBody))]
        public async Task<DataMatchUploadResponseBody> PostMatchedDataFiles2([FromBody] DataMatchUploadRequestBody UploadedFile)
        {
            DataMatchUploadResponseBody _dMUResponse = new DataMatchUploadResponseBody() { responseheader = new ResponseHeader() };
            if (!ModelState.IsValid)
            {
                _dMUResponse.responseheader.orderid = (UploadedFile.requestheader.orderid == null) ? string.Empty : UploadedFile.requestheader.orderid;
                ResponseDetail _responseDetail = new ResponseDetail();
                _responseDetail.id = "1";
                _responseDetail.requestData = UploadedFile.requestdetail;
                _responseDetail.errorData = new List<ErrorData>();

                foreach (var field in ModelState.Keys)
                {
                    if (ModelState[field].Errors != null)
                    {
                        foreach (var _error in ModelState[field].Errors)
                        {
                            ErrorData _errorData = new ErrorData()
                            {
                                errorCause = "Invalid Request",
                                errorField = field,
                                errorExplanation = _error.ErrorMessage,
                                errorValidationType = ""
                            };
                            _responseDetail.errorData.Add(_errorData);
                        }
                    }
                }
                _dMUResponse.responsedetail = new List<ResponseDetail>();
                _dMUResponse.responsedetail.Add(_responseDetail);
                return _dMUResponse;
            }
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string businessId = string.Empty;
            string fileId = string.Empty;
            if (headers.Contains("businessId"))
            {
                businessId = headers.GetValues("businessId").FirstOrDefault();
            }
            if (headers.Contains("businessId"))
            {
                fileId = headers.GetValues("fileId").FirstOrDefault();
            }
            return _dMUResponse;
            //return GenerateResponse(await _dataMatchUploadResponse.UploadDataMatchFile(UploadedFile, businessId, fileId));
        }

        [HttpPost]
        [Route("3")]
        [ResponseType(typeof(DataMatchUploadResponse))]
        public async Task<DataMatchUploadResponse> PostMatchedDataFiles3([FromBody] DataMatchUploadRequestBody UploadedFile)
        {
            DataMatchUploadResponse _dataMatchUploadResponse = new DataMatchUploadResponse();
            if (!ModelState.IsValid)
            {
                foreach (var field in ModelState.Keys)
                {
                    if (ModelState[field].Errors != null)
                    {

                    }
                }
                return new DataMatchUploadResponse()
                {
                    result = "Invalid Request",
                    status = "Failure",
                    statusCode = 500
                };
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string businessId = string.Empty;
            string fileId = string.Empty;
            if (headers.Contains("businessId"))
            {
                businessId = headers.GetValues("businessId").FirstOrDefault();
            }
            if (headers.Contains("businessId"))
            {
                fileId = headers.GetValues("fileId").FirstOrDefault();
            }
            return new DataMatchUploadResponse();
            //return GenerateResponse(await _dataMatchUploadResponse.UploadDataMatchFile(UploadedFile, businessId, fileId));
        }

        private DataMatchUploadResponse GenerateResponse(DataMatchUploadResponse response, ModelStateDictionary modelState = null)
        {
            //DataMatchUploadResponse _dataMatchUploadResponse;
            //if (response.statusCode == 400)
            //    _dataMatchUploadResponse = new DataMatchUploadResponse() {result = "", statusCode = response.statusCode }//Request.CreateResponse(HttpStatusCode.BadRequest, response);

            //if (response.statusCode == 409)
            //    result = Request.CreateResponse(HttpStatusCode.Conflict, response);

            //if (response.statusCode == 202)
            //    result = Request.CreateResponse(HttpStatusCode.Accepted, response);

            //if (response.statusCode == 500)
            //    if (modelState != null)
            //    {
            //        //if (!modelState.IsValid)
            //        //{
            //        //    return new DataMatchUploadResponse();
            //        //}
            //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            //    }
            //    else
            //    {
            //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            //    }

            //return result;

            return new DataMatchUploadResponse();
        }

        public List<ErrorData> Validate(DataMatchUploadRequestBody dataMatchUploadRequestBody)
        {
            List<ErrorData> _errors = new List<ErrorData>();
            
            if (dataMatchUploadRequestBody.requestheader.orderid == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderid",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "orderId field is not present",
                    errorValidationType = "MISSING"
                });
            }
            else
            {
                if (dataMatchUploadRequestBody.requestheader.orderid == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "orderid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in orderId field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
            }

            if (dataMatchUploadRequestBody.requestheader.ordertype == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderType",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "orderType field is not present",
                    errorValidationType = "MISSING"
                });
            }
            else
            {
                if (!((dataMatchUploadRequestBody.requestheader.ordertype.ToLower() == "new") || 
                    (dataMatchUploadRequestBody.requestheader.ordertype.ToLower() == "existing")))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "orderType",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in orderType field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
            }
            if (dataMatchUploadRequestBody.requestheader.ordertype.ToLower() == "new" && 
                (GetFromSQL("select ordertype from BLOB where orderid = " + dataMatchUploadRequestBody.requestheader.orderid).Rows.Count > 0))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderId",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in orderId field is duplicate",
                    errorValidationType = "MISSING"
                });
            }
            if (dataMatchUploadRequestBody.requestheader.ordertype.ToLower() == "existing" && 
                (GetFromSQL("select ordertype from BLOB where orderid = " + dataMatchUploadRequestBody.requestheader.orderid).Rows.Count == 0))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderid",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in orderId field does not exist",
                    errorValidationType = "MISSING"
                });
            }
            
            if (dataMatchUploadRequestBody.requestheader.businessid == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "businessid",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "businessid field is not present",
                    errorValidationType = "MISSING"
                });
            }
            else
            {
                if (dataMatchUploadRequestBody.requestheader.businessid == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "businessid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in businessid field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
            }

            if (dataMatchUploadRequestBody.requestheader.matchtype == string.Empty)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "matchtype",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in matchtype is not valid",
                    errorValidationType = "INVALID"
                });
            }
            
            if (dataMatchUploadRequestBody.requestheader.noofrecords == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "noofrecords",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "noofrecords is not present",
                    errorValidationType = "MISSING"
                });
            }
            else
            {
                if (dataMatchUploadRequestBody.requestheader.noofrecords == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "noofrecords",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in noofrecords field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
            }

            if (dataMatchUploadRequestBody.requestheader.email == string.Empty)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "email",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in email field is not valid",
                    errorValidationType = "INVALID"
                });
            }

            if (_errors.Count > 0){ return _errors; }

            foreach (Requestdetail _requestDetail in dataMatchUploadRequestBody.requestdetail)
            {
                if (_requestDetail.id == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "id is not present",
                        errorValidationType = "MISSING"
                    });
                }

                if (_requestDetail.requesttype == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "requestType is not present",
                        errorValidationType = "MISSING"
                    });
                }

                if (_requestDetail.requesttype == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in requestType field is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if ((dataMatchUploadRequestBody.requestheader.ordertype.ToLower() == "new") && 
                    ((_requestDetail.requesttype.ToLower() == "link") || (_requestDetail.requesttype.ToLower() == "update")))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in requestType field cannot be 'update' or 'link' ",
                        errorValidationType = "INVALID"
                    });
                }

                if ((dataMatchUploadRequestBody.requestheader.ordertype.ToLower() == "existing") &&
                    (_requestDetail.requesttype.ToLower() == "update") &&
                    (!GetFromSQL("select trackid from BLOB where id = " + _requestDetail.id).Rows.Contains(_requestDetail.trackid)))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field for requesting updates does not exist",
                        errorValidationType = "INVALID"
                    });
                }

                if (((_requestDetail.requesttype.ToLower() == "update") || (_requestDetail.requesttype.ToLower() == "link")) &&
                    (_requestDetail.trackid == null))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field for requesting updates is not for a purchased record",
                        errorValidationType = "INVALID"
                    });
                }
            }

            return _errors;
        }

        public DataTable GetFromSQL(string query)
        {
            DataTable _dataTable = new DataTable();
            SqlConnection _sqlConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\DOMINIC\\GITHUB\\Source\\Repos\\DominicLutherEarl\\MCWebApplication\\MCAPI\\App_Data\\MCAPIDB.mdf;Integrated Security=True");
            SqlCommand _sqlCommand = new SqlCommand(query, _sqlConnection);
            SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
            _sqlConnection.Open();
            _sqlDataAdapter.Fill(_dataTable);
            _sqlConnection.Close();
            _sqlDataAdapter.Dispose();
            return _dataTable;
        }
    }
}