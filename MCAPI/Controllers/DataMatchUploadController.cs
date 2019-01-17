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
        [HttpPost]
        [Route("PostMatchedDataFiles")]
        [ResponseType(typeof(DataMatchUploadResponse))]
        public async Task<DataMatchUploadResponse> PostMatchedDataFiles([FromBody] DataMatchUploadRequest dataMatchUploadRequest)
        {
            DataMatchUploadResponse _dataMatchUploadResponse = new DataMatchUploadResponse();
            _dataMatchUploadResponse.ResponseHeader = dataMatchUploadRequest.RequestHeader;
            _dataMatchUploadResponse.ResponseDetails = dataMatchUploadRequest.RequestDetails;
            _dataMatchUploadResponse = Validate(_dataMatchUploadResponse);
            if (_dataMatchUploadResponse.ResponseHeader.errorData.Count > 0 || _dataMatchUploadResponse.ResponseHeader.matchStatistics.errorRecords > 0)
            {
                return _dataMatchUploadResponse;
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
            InsertIntoSQL(dataMatchUploadRequest);
            return _dataMatchUploadResponse;
            //return GenerateResponse(await _dataMatchUploadResponse.UploadDataMatchFile(UploadedFile, businessId, fileId));
        }
        
        //[HttpPost]
        //[Route("2")]
        //[ResponseType(typeof(DataMatchUploadResponseBody))]
        //public async Task<DataMatchUploadResponseBody> PostMatchedDataFiles2([FromBody] DataMatchUploadRequestBody UploadedFile)
        //{
        //    DataMatchUploadResponseBody _dMUResponse = new DataMatchUploadResponseBody() { responseheader = new ResponseHeader() };
        //    if (!ModelState.IsValid)
        //    {
        //        _dMUResponse.responseheader.orderid = (UploadedFile.requestheader.orderid == null) ? string.Empty : UploadedFile.requestheader.orderid;
        //        ResponseDetail _responseDetail = new ResponseDetail();
        //        _responseDetail.id = "1";
        //        _responseDetail.requestData = UploadedFile.requestdetail;
        //        _responseDetail.errorData = new List<ErrorData>();

        //        foreach (var field in ModelState.Keys)
        //        {
        //            if (ModelState[field].Errors != null)
        //            {
        //                foreach (var _error in ModelState[field].Errors)
        //                {
        //                    ErrorData _errorData = new ErrorData()
        //                    {
        //                        errorCause = "Invalid Request",
        //                        errorField = field,
        //                        errorExplanation = _error.ErrorMessage,
        //                        errorValidationType = ""
        //                    };
        //                    _responseDetail.errorData.Add(_errorData);
        //                }
        //            }
        //        }
        //        _dMUResponse.responsedetail = new List<ResponseDetail>();
        //        _dMUResponse.responsedetail.Add(_responseDetail);
        //        return _dMUResponse;
        //    }
        //    System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
        //    string businessId = string.Empty;
        //    string fileId = string.Empty;
        //    if (headers.Contains("businessId"))
        //    {
        //        businessId = headers.GetValues("businessId").FirstOrDefault();
        //    }
        //    if (headers.Contains("businessId"))
        //    {
        //        fileId = headers.GetValues("fileId").FirstOrDefault();
        //    }
        //    return _dMUResponse;
        //    //return GenerateResponse(await _dataMatchUploadResponse.UploadDataMatchFile(UploadedFile, businessId, fileId));
        //}
        
        //private DataMatchUploadResponse GenerateResponse(DataMatchUploadResponse response, ModelStateDictionary modelState = null)
        //{
        //    //DataMatchUploadResponse _dataMatchUploadResponse;
        //    //if (response.statusCode == 400)
        //    //    _dataMatchUploadResponse = new DataMatchUploadResponse() {result = "", statusCode = response.statusCode }//Request.CreateResponse(HttpStatusCode.BadRequest, response);

        //    //if (response.statusCode == 409)
        //    //    result = Request.CreateResponse(HttpStatusCode.Conflict, response);

        //    //if (response.statusCode == 202)
        //    //    result = Request.CreateResponse(HttpStatusCode.Accepted, response);

        //    //if (response.statusCode == 500)
        //    //    if (modelState != null)
        //    //    {
        //    //        //if (!modelState.IsValid)
        //    //        //{
        //    //        //    return new DataMatchUploadResponse();
        //    //        //}
        //    //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
        //    //    }
        //    //    else
        //    //    {
        //    //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
        //    //    }

        //    //return result;

        //    return new DataMatchUploadResponse();
        //}
        public DataMatchUploadResponse Validate(DataMatchUploadResponse dataMatchUploadResponse)
        {
            List<ErrorData> _errors = new List<ErrorData>();
            int _totalRecords = dataMatchUploadResponse.ResponseDetails.Count;
            int _errorRecords = 0;
            dataMatchUploadResponse.ResponseHeader.errorData.Clear();
            if (dataMatchUploadResponse.ResponseHeader.orderid == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderid",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "orderId field is not present",
                    errorValidationType = "MISSING"
                });
                dataMatchUploadResponse.ResponseHeader.orderid = "null";
            }
            else
            {
                if (dataMatchUploadResponse.ResponseHeader.orderid == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "orderid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in orderId field is not valid",
                        errorValidationType = "INVALID"
                    });
                    dataMatchUploadResponse.ResponseHeader.orderid = "null";
                }
            }

            if (dataMatchUploadResponse.ResponseHeader.ordertype == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderType",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "orderType field is not present",
                    errorValidationType = "MISSING"
                });
                dataMatchUploadResponse.ResponseHeader.ordertype = "null";
            }
            else
            {
                if (!((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new") || 
                    (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing")))
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
            if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new" && 
                (GetFromSQL("select ordertype from BLOB where orderid = " + dataMatchUploadResponse.ResponseHeader.orderid).Rows.Count > 0))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderId",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in orderId field is duplicate",
                    errorValidationType = "MISSING"
                });
            }
            if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" && 
                (GetFromSQL("select ordertype from BLOB where orderid = " + dataMatchUploadResponse.ResponseHeader.orderid).Rows.Count == 0))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderid",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in orderId field does not exist",
                    errorValidationType = "MISSING"
                });
            }
            
            if (dataMatchUploadResponse.ResponseHeader.businessid == null)
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
                if (dataMatchUploadResponse.ResponseHeader.businessid == string.Empty)
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

            if (dataMatchUploadResponse.ResponseHeader.matchtype == string.Empty)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "matchtype",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in matchtype is not valid",
                    errorValidationType = "INVALID"
                });
            }
            
            if (dataMatchUploadResponse.ResponseHeader.noofrecords == null)
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
                if (dataMatchUploadResponse.ResponseHeader.noofrecords == string.Empty)
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

            if (dataMatchUploadResponse.ResponseHeader.email == string.Empty)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "email",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in email field is not valid",
                    errorValidationType = "INVALID"
                });
            }

            if (_errors.Count > 0)
            {
                foreach (ErrorData _error in _errors)
                {
                    dataMatchUploadResponse.ResponseHeader.errorData.Add(_error);
                }                
            }

            for (int i=0; i< _totalRecords; i++)
            {
                _errors = new List<ErrorData>();
                dataMatchUploadResponse.ResponseDetails[i].errorData.Clear();
                if (dataMatchUploadResponse.ResponseDetails[i].id == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "id is not present",
                        errorValidationType = "MISSING"
                    });
                }

                if (dataMatchUploadResponse.ResponseDetails[i].requesttype == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "requestType is not present",
                        errorValidationType = "MISSING"
                    });
                }

                if (dataMatchUploadResponse.ResponseDetails[i].requesttype == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in requestType field is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if ((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new") &&
                    ((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link") || 
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update")))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in requestType field cannot be 'update' or 'link' ",
                        errorValidationType = "INVALID"
                    });
                }

                if ((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing") &&
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") &&
                    (!GetFromSQL("select trackid from BLOB where id = " + dataMatchUploadResponse.ResponseDetails[i].id).Rows.Contains(dataMatchUploadResponse.ResponseDetails[i].trackid)))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field for requesting updates does not exist",
                        errorValidationType = "INVALID"
                    });
                }

                if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") || 
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                    (dataMatchUploadResponse.ResponseDetails[i].trackid == null))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field for requesting updates is not for a purchased record",
                        errorValidationType = "MISSING"
                    });
                }

                if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") || 
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                    (dataMatchUploadResponse.ResponseDetails[i].trackid == string.Empty))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") || 
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                    (dataMatchUploadResponse.ResponseDetails[i].trackid == string.Empty))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseDetails[i].companyname == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "companyName",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "companyName is not present",
                        errorValidationType = "MISSING"
                    });
                }
                if (dataMatchUploadResponse.ResponseDetails[i].monitoringType == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "monitoringType",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in monitoring field is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
                    dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link" &&
                    dataMatchUploadResponse.ResponseDetails[i].trackid != null && 
                    dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance[0].referenceId != null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "link",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "trackId and referenceId should not be provided in the same request for linking",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
                    dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link" &&
                    !(GetFromSQL("select * from BLOB where orderid = '" + dataMatchUploadResponse.ResponseHeader.orderid + "' and trackid = '" + dataMatchUploadResponse.ResponseDetails[i].trackid + "'").Rows.Count > 0))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in trackId field to link a request record to a trade directory record is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
                    dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link" &&
                    dataMatchUploadResponse.ResponseDetails[i].trackid == null && 
                    dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance[0].referenceId == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requestType",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "trackId or referenceId not present in the request for linking",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
                    dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link" &&
                    !(GetFromSQL("select * from BLOB where orderid = '" + dataMatchUploadResponse.ResponseHeader.orderid + "' and referenceid = '" + dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance[0].referenceId + "'").Rows.Count > 0))
                    if (_errors.Count >0)
                    {
                        dataMatchUploadResponse.ResponseDetails[i].errorData.Concat(_errors);
                        _errorRecords++;
                    }
            }
            dataMatchUploadResponse.ResponseHeader.matchStatistics.errorRecords = _errorRecords;
            return dataMatchUploadResponse;
        }

        public DataTable GetFromSQL(string query)
        {
            DataTable _dataTable = new DataTable();
            SqlConnection _sqlConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\DOMINIC\\GITHUB\\Source\\Repos\\DominicLutherEarl\\MCWebApplication\\MCAPI\\App_Data\\MCAPIDB.mdf;Integrated Security=True");
            try
            {
                SqlCommand _sqlCommand = new SqlCommand(query, _sqlConnection);
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
                _sqlConnection.Open();
                _sqlDataAdapter.Fill(_dataTable);
                _sqlConnection.Close();
                _sqlDataAdapter.Dispose();
            }
            catch (Exception _exception)
            {

            }
            finally
            {
                if (_sqlConnection.State != ConnectionState.Closed)
                {
                    _sqlConnection.Close();
                }
            }
            return _dataTable;
        }

        public bool InsertIntoSQL(DataMatchUploadRequest dataMatchUploadRequest)
        {
            SqlConnection _sqlConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\DOMINIC\\GITHUB\\Source\\Repos\\DominicLutherEarl\\MCWebApplication\\MCAPI\\App_Data\\MCAPIDB.mdf;Integrated Security=True");
            SqlCommand _sqlCommand;
            try
            {
                foreach (Detail _detail in dataMatchUploadRequest.RequestDetails)
                {
                    string _insertQuery = "INSERT INTO [dbo].[BLOB] ([orderid], [ordertype], [businessid], [matchtype], [noofrecords], [email], " +
                        "[id], [requesttype], [trackid], [companyname], [address1], [address2], [address3], [address4], [city], [state], [country], [zip], " +
                        "[phone], [url], [contact], [ein], [tin], [vat], [registrationnumber], [monitoringType], [updatetype], [linktrackid], [referenceid], [CustomFields]) " +
                        "VALUES ( " +
                        " N'" + (dataMatchUploadRequest.RequestHeader.orderid)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.ordertype)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.businessid)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.matchtype)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.noofrecords)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.email)
                    + "', N'" + (_detail.id)
                    + "', N'" + (_detail.requesttype)
                    + "', N'" + (_detail.trackid)
                    + "', N'" + (_detail.companyname)
                    + "', N'" + (_detail.address.address1)
                    + "', N'" + (_detail.address.address2)
                    + "', N'" + (_detail.address.address3)
                    + "', N'" + (_detail.address.address4)
                    + "', N'" + (_detail.address.city)
                    + "', N'" + (_detail.address.state)
                    + "', N'" + (_detail.address.country)
                    + "', N'" + (_detail.address.zip)
                    + "', N'" + (_detail.phone)
                    + "', N'" + (_detail.url)
                    + "', N'" + (_detail.contact)
                    + "', N'" + (_detail.ein)
                    + "', N'" + (_detail.tin)
                    + "', N'" + (_detail.vat)
                    + "', N'" + (_detail.registrationnumber)
                    + "', N'" + (_detail.monitoringType)
                    + "', N'" + (_detail.updatetype)
                    + "', N'" + (_detail.linking.linktrackid)
                    + "', N'" + (_detail.linking.linkcompliance[0].referenceId)
                    + "', N'" + (_detail.customfields)
                    + "')";
                    _sqlCommand = new SqlCommand(_insertQuery, _sqlConnection);
                    _sqlConnection.Open();
                    _sqlCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception _exception)
            {
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}