using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MC.Track.FileValidationAPI
{
    public class FileValidationController : ApiController
    {
        private static readonly List<DataMatchUploadRequest> UploadRequests = new List<DataMatchUploadRequest>();

        [HttpPost]
        [ResponseType(typeof(DataMatchUploadResponse))]
        public async Task<DataMatchUploadResponse> Post([FromBody] DataMatchUploadRequest dataMatchUploadRequest)
        {
            DataMatchUploadResponse dataMatchUploadResponse = new DataMatchUploadResponse();
            dataMatchUploadResponse.fileId = GetFileIdFromHeader();
            dataMatchUploadResponse.businessId = GetBusinessIdFromHeader();
            dataMatchUploadResponse.ResponseHeader = new ResponseHeader(dataMatchUploadRequest.RequestHeader);
            dataMatchUploadResponse.ResponseDetails = new List<ResponseDetail>();
            foreach (Detail detail in dataMatchUploadRequest.RequestDetail)
            {
                ResponseDetail responseDetail = new ResponseDetail(detail);
                dataMatchUploadResponse.ResponseDetails.Add(responseDetail);
            }
            dataMatchUploadResponse = Validate(dataMatchUploadResponse);
            AddRecord(dataMatchUploadRequest);
            return dataMatchUploadResponse;
        }
        private string GetFileIdFromHeader()
        {
            var fileIdHeader = GetHeader("fileId");
            return fileIdHeader?.Value?.FirstOrDefault();
        }
        private string GetBusinessIdFromHeader()
        {
            var businessIdHeader = GetHeader("businessId");
            return businessIdHeader?.Value?.FirstOrDefault();
        }
        private KeyValuePair<string, IEnumerable<string>>? GetHeader(string headerKey)
        {
            return Request?.Headers?.FirstOrDefault(h => h.Key.Equals(headerKey, StringComparison.InvariantCultureIgnoreCase));
        }
        private DataMatchUploadResponse Validate(DataMatchUploadResponse dataMatchUploadResponse)
        {
            List<ErrorData> _errors = new List<ErrorData>();
            int _totalRecords = dataMatchUploadResponse.ResponseDetails.Count;
            int _errorRecords = 0;

            #region HeaderValidations
            if (dataMatchUploadResponse.ResponseHeader.orderid == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderId",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "orderId field is not present",
                    errorValidationType = Constants.MissingFieldCategory
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
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in orderId field is not valid",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                    dataMatchUploadResponse.ResponseHeader.orderid = "null";
                }
            }

            if (dataMatchUploadResponse.ResponseHeader.ordertype == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderType",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "orderType field is not present",
                    errorValidationType = Constants.MissingFieldCategory
                });
                dataMatchUploadResponse.ResponseHeader.ordertype = "null";
            }
            if (dataMatchUploadResponse.ResponseHeader.ordertype != null &&
            !(dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() =="new" ||
            dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing"))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderType",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "value provided in orderType field is not valid",
                    errorValidationType = Constants.InvalidFieldCategory
                });
            }
            if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new" &&
            OrderIdExists(dataMatchUploadResponse.ResponseHeader.orderid))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderId",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "value provided in orderId field is duplicate",
                    errorValidationType = Constants.InvalidFieldCategory
                });
            }

            if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
            !OrderIdExists(dataMatchUploadResponse.ResponseHeader.orderid))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderid",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "value provided in orderId field does not exist",
                    errorValidationType = Constants.MissingFieldCategory
                });
            }

            if (dataMatchUploadResponse.ResponseHeader.businessid == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "businessId",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "businessId field is not present",
                    errorValidationType = Constants.MissingFieldCategory
                });
            }
            else
            {
                if (dataMatchUploadResponse.ResponseHeader.businessid == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "businessId",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in businessId field is not valid",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }
            }

            if (dataMatchUploadResponse.ResponseHeader.matchtype == string.Empty)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "matchType",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "value provided in matchType is not valid",
                    errorValidationType = Constants.InvalidFieldCategory
                });
            }

            if (dataMatchUploadResponse.ResponseHeader.noofrecords == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "noofRecords",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "noofRecords is not present",
                    errorValidationType = Constants.MissingFieldCategory
                });
            }
            else
            {
                if (dataMatchUploadResponse.ResponseHeader.noofrecords == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "noofRecords",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in noofRecords field is not valid",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }
            }


            if (!IsValidEmail(dataMatchUploadResponse.ResponseHeader.email))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "email",
                    errorCause = Constants.ErrorCause,
                    errorExplanation = "value provided in email field is not valid",
                    errorValidationType = Constants.InvalidFieldCategory
                });
            }

            if (_errors.Any())
            {
                dataMatchUploadResponse.ResponseHeader.errorData.AddRange(_errors);
                dataMatchUploadResponse.ResponseDetails = null;
                return dataMatchUploadResponse;
            }
            #endregion

            #region DetailValidation
            for (int i = 0; i < _totalRecords; i++)
            {
                _errors = new List<ErrorData>();
                if (dataMatchUploadResponse.ResponseDetails[i].errorData == null)
                {
                    dataMatchUploadResponse.ResponseDetails[i].errorData = new List<ErrorData>();
                }
                dataMatchUploadResponse.ResponseDetails[i].errorData.Clear();
                if (dataMatchUploadResponse.ResponseDetails[i].id == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "id is not present",
                        errorValidationType = Constants.MissingFieldCategory
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype == "new" &&
                dataMatchUploadResponse.ResponseDetails[i].id != null &&
                (dataMatchUploadResponse.ResponseDetails.Where(_ => _.id == dataMatchUploadResponse.ResponseDetails[i].id).Count() > 1))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in id field is duplicate",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype == "existing" &&
                OrderIdAndIdExists(dataMatchUploadResponse.ResponseHeader.orderid, dataMatchUploadResponse.ResponseDetails[i].id))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in id field does not exist in the order",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                if (dataMatchUploadResponse.ResponseDetails[i].requesttype == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "requestType is not present",
                        errorValidationType = Constants.MissingFieldCategory
                    });
                    dataMatchUploadResponse.ResponseDetails[i].requesttype = string.Empty;
                }
                else if (dataMatchUploadResponse.ResponseDetails[i].requesttype == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in requestType field is not valid",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                if ((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new") &&
                ((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link") ||
                (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update")))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in requestType field cannot be 'update' or 'link' ",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                if ((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing") &&
                (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") &&
                !IdTrackIdExists(dataMatchUploadResponse.ResponseDetails[i].id, dataMatchUploadResponse.ResponseDetails[i].trackid))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in the trackId field for requesting updates does not exist",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                //if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") ||
                // (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                // (dataMatchUploadResponse.ResponseDetails[i].trackid == null))
                //{
                // _errors.Add(new ErrorData()
                // {
                // errorField = "trackid",
                // errorCause = CONSTANTS.ErrorCause,
                // errorExplanation = "value provided in the trackId field for requesting updates is not for a purchased record",
                // errorValidationType = CONSTANTS.MissingFieldCategory
                // });
                //}

                if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") ||
                (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                (dataMatchUploadResponse.ResponseDetails[i].trackid == null))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "trackId is not present",
                        errorValidationType = Constants.MissingFieldCategory
                    });
                }
                else if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") ||
                (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                (dataMatchUploadResponse.ResponseDetails[i].trackid == string.Empty))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in the trackId field is not valid",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                if (dataMatchUploadResponse.ResponseDetails[i].companyname == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "companyName",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "companyName is not present",
                        errorValidationType = Constants.MissingFieldCategory
                    });
                }
                if (dataMatchUploadResponse.ResponseDetails[i].monitoringType == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "monitoringType",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "value provided in monitoring field is not valid",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
                dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link" &&
                dataMatchUploadResponse.ResponseDetails[i].trackid == null &&
                dataMatchUploadResponse.ResponseDetails[i].linking != null &&
                dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance.Any() &&
                string.IsNullOrEmpty(dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance[0].referenceId))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requestType",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "trackId or referenceId not present in the request for linking",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
                dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link" &&
                dataMatchUploadResponse.ResponseDetails[i].trackid != null &&
                dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance != null &&
                dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance.Count > 0 &&
                dataMatchUploadResponse.ResponseDetails[i].linking.linkcompliance[0].referenceId != null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "link",
                        errorCause = Constants.ErrorCause,
                        errorExplanation = "trackId and referenceId should not be provided in the same request for linking",
                        errorValidationType = Constants.InvalidFieldCategory
                    });
                }
                if (_errors.Any())
                {
                    dataMatchUploadResponse.ResponseDetails[i].errorData.AddRange(_errors);
                    _errorRecords++;
                }
            }
            if (dataMatchUploadResponse.ResponseHeader.matchStatistics == null)
            {
                dataMatchUploadResponse.ResponseHeader.matchStatistics = new matchStatistics();
            }
            dataMatchUploadResponse.ResponseHeader.matchStatistics.totalRecords = _totalRecords;
            dataMatchUploadResponse.ResponseHeader.matchStatistics.errorRecords = _errorRecords;
            #endregion
            return dataMatchUploadResponse;
        }
        private bool OrderIdExists(string orderId)
        {
            return UploadRequests.Where(r => r.RequestHeader?.orderid == orderId)
            .Any();
        }
        private bool IdTrackIdExists(string id, string trackId)
        {
            return UploadRequests.SelectMany(r => r.RequestDetail ?? new List<Detail>())
            .Where(rd => rd?.id == id && rd.trackid == trackId)
            .Any();
        }
        private bool OrderIdTrackIdExists(string orderId, string trackId)
        {
            return UploadRequests.Where(r => r.RequestHeader?.orderid == orderId)
            .SelectMany(r => r.RequestDetail ?? new List<Detail>())
            .Where(rd => rd?.trackid == trackId)
            .Any();
        }
        private bool OrderIdAndIdExists(string orderId, string id)
        {
            return UploadRequests.Where(r => r.RequestHeader?.orderid == orderId)
            .SelectMany(r => r.RequestDetail ?? new List<Detail>())
            .Where(rd => rd?.id == id)
            .Any();
        }
        private void AddRecord(DataMatchUploadRequest dataMatchUploadRequest)
        {
            if (dataMatchUploadRequest == null)
            {
                return;
            }
            UploadRequests.Add(dataMatchUploadRequest);
        }
        //Sourced from https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        //and https://stackoverflow.com/questions/16167983/best-regular-expression-for-email-validation-in-c-sharp
        //and https://azuliadesigns.com/validate-email-addresses/
        // ****As recommended by all sources in public, the best way to validate an email address is to send an email to it.****
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                return false;
            }
            try
            {             
                System.Net.Mail.MailAddress _mailAddress = new System.Net.Mail.MailAddress(email);
            }
            catch (Exception e)
            {
                return false;
            }
            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    var domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
