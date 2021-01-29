using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

// NuGet packages:
// Microsoft.Extensions.Configuration.Binder
// Microsoft.Extensions.Configuration.Json
// Twilio

namespace TwilioLibrary
{
    public class SendMessages
    {
        public IConfiguration config;
        public SendMessages()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            config = builder.Build();
        }

        public string GetAccountSid()
        {
            var mydata = config.GetSection("Settings").Get<UserDataTwilioAccount>();
            return mydata.ACCOUNT_SID;
        }

        public string GetToken()
        {
            var mydata = config.GetSection("Settings").Get<UserDataTwilioAccount>();
            return mydata.AUTH_TOKEN;
        }

        public string GetMessagingServiceSid()
        {
            var mydata = config.GetSection("Settings").Get<UserDataTwilioAccount>();
            return mydata.MessagingServiceSid;
        }

        public string GetPhoneFrom()
        {
            var mydata = config.GetSection("Settings").Get<UserDataTwilioAccount>();
            return mydata.PhoneFrom;
        }

        public string SendWhatsAppmessage(string ReceiverNumber, string MessageBody)
        {
            TwilioClient.Init(GetAccountSid(), GetToken());
            string phoneNumber = GetPhoneFrom();

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:" + ReceiverNumber));
            messageOptions.From = new PhoneNumber("whatsapp:"+ phoneNumber);
            messageOptions.Body = MessageBody;

            var message = MessageResource.Create(messageOptions);

            return message.Sid;
        }

        public string SendSMSMessage(string ReceiverNumber, string MessageBody)
        {
            TwilioClient.Init(GetAccountSid(), GetToken());

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(ReceiverNumber));
            messageOptions.MessagingServiceSid = GetMessagingServiceSid();
            messageOptions.Body = MessageBody;

            var message = MessageResource.Create(messageOptions);
            return message.Sid;
        }
    }    
}
