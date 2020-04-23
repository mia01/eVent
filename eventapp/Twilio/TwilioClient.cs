using Microsoft.Extensions.Configuration;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace eventapp.Twilio
{
    public class TwilioClient
    {
        private readonly ITwilioRestClient _client;
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioNumber;

        public TwilioClient(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSID"];
            _authToken = configuration["Twilio:AuthToken"];
            _twilioNumber = configuration["Twilio:TwilioNumber"];
            _client = new TwilioRestClient(_accountSid, _authToken);

        }

        public void SendSmsMessage(string phoneNumber, string message)
        {
            var to = new PhoneNumber(phoneNumber);
            MessageResource.Create(
                to,
                from: new PhoneNumber(_twilioNumber),
                body: message,
                client: _client);
        }
    }
}
