﻿using Google.GData.Client;

namespace Blogger365
{
    public class ServiceHandle
    {

        private Service gClientService;
        public Service GoogleService
        {
            get { return gClientService; }
            private set { gClientService = value; }
        }

        public string UserName { get; private set; }
        private string authToken;

        public string AuthenticationToken
        {
            get { return authToken; }
            private set { authToken = value; }
        }
        
        public ServiceHandle(Service service)
        {
            gClientService = service;
        }

        public string Login(string username, string password)
        {
            try
            {
                UserName = username;
                gClientService.setUserCredentials(username, password);
                authToken = gClientService.QueryClientLoginToken();

                return AuthenticationToken;
            }
            catch (InvalidCredentialsException iex) { return null; }
        }

    }
}
