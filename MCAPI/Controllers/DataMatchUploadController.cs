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

namespace Mc.TD.Upload.Api.Controllers
{
    public class DataMatchUploadController : ApiController
    {
        public IDataMatchUploadService _dataMatchUploadResponse;
        public IDataMatchUploadService _dMUResponse;

        public DataMatchUploadController()
        {
        
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
    }
}