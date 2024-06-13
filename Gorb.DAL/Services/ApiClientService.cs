using Gorb.DAL.DataContracts;
using Gorb.DAL.DataContracts.FriendshipData;
using Gorb.DAL.DataContracts.UserData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gorb.DAL.Services
{
    public class ApiClientService
    {
        public const string AutorizedHttpClient = "AutorizedHttpClient";

        private readonly ILogger<ApiClientService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _serverAddress;

        public ApiClientService(ILogger<ApiClientService> logger,
            IHttpClientFactory httpClientFactory,
            string serverAddress)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _serverAddress = $"{serverAddress}/api";

        }

        #region Auth

        public async Task<LoginResponse> LoginAsync(string username, string password,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient()
                .PostAsJsonAsync($"{_serverAddress}/auth/login", new LoginRequest
                {
                    Nickname = username,
                    Password = password
                },
                    cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content
                    .ReadFromJsonAsync<LoginResponse>(
                        cancellationToken: cancellationToken);

                if (responseContent == null)
                {
                    throw new JsonException("Response has unknown format");
                }

                return responseContent;
            }
            else
            {
                throw new HttpRequestException(
                    $"Error logging in: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }
        }

        public async Task<bool> RegisterAsync(string username, string password, int avatar, int notificationTimeOut,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient()
                .PostAsJsonAsync($"{_serverAddress}/auth/register", new RegisterRequest
                {
                    Nickname = username,
                    Password = password,
                    Avatar = avatar,
                    NotificationTimeOut = notificationTimeOut,
                },
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Error logging in: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            return true;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(string accessToken,
            string refreshToken, CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient()
                .PostAsJsonAsync($"{_serverAddress}/auth/refresh-token", new RefreshTokenRequest
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                },
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Refresh token fails: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            var responseContent = await response.Content
                .ReadFromJsonAsync<RefreshTokenResponse>(
                    cancellationToken: cancellationToken);

            if (responseContent == null)
            {
                throw new JsonException("Response has unknown format");
            }

            return responseContent;
        }

        public async Task<bool> ValidateTokenAsync(
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient(AutorizedHttpClient)
                .GetAsync($"{_serverAddress}/auth/validate-token", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Validate token fails: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            return true;
        }

        public async Task<bool> LogoutAsync(
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient(AutorizedHttpClient)
                .GetAsync($"{_serverAddress}/auth/logout", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Logout fails: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            return true;
        }

        #endregion
        #region Friends
        public async Task<FriendsResponse> GetMyFriendsAsync(
        CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient(AutorizedHttpClient)
                .GetAsync($"{_serverAddress}/FriendshipData/friends", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Get teams request fails: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            var responseContent = await response.Content
                .ReadFromJsonAsync<FriendsResponse>(
                    cancellationToken: cancellationToken);

            if (responseContent == null)
            {
                throw new JsonException("Response has unknown format");
            }

            return responseContent;
        }
        public async Task<FriendsResponse> AddFriendAsync(int id,
        CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient(AutorizedHttpClient)
                .PostAsJsonAsync($"{_serverAddress}/FriendshipData/friends",
                new FriendshipRequest { 
                    FriendId = id
                },
                cancellationToken);


            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Get teams request fails: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            var responseContent = await response.Content
                .ReadFromJsonAsync<FriendsResponse>(
                    cancellationToken: cancellationToken);

            if (responseContent == null)
            {
                throw new JsonException("Response has unknown format");
            }

            return responseContent;
        }
        #endregion
        #region Exp
        public async Task<UserExperienceResponse> GetExperienceAsync(
        CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient(AutorizedHttpClient)
                .GetAsync($"{_serverAddress}/UserData/experience", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Get teams request fails: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            var responseContent = await response.Content
                .ReadFromJsonAsync<UserExperienceResponse>(
                    cancellationToken: cancellationToken);

            if (responseContent == null)
            {
                throw new JsonException("Response has unknown format");
            }

            return responseContent;
        }
        public async Task<UserExperienceWithLvlResponse> SetExperienceAsync(int amountofexperience,
        CancellationToken cancellationToken = default)
        {
            var response = await _httpClientFactory
                .CreateClient(AutorizedHttpClient)
                .PostAsJsonAsync($"{_serverAddress}/UserData/experience", 
                new UserExperienceRequest 
                { 
                    AmountExperience = amountofexperience 
                }, 
                cancellationToken);


            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Get teams request fails: {response.ReasonPhrase}", null,
                    response.StatusCode);
            }

            var responseContent = await response.Content
                .ReadFromJsonAsync<UserExperienceWithLvlResponse>(
                    cancellationToken: cancellationToken);

            if (responseContent == null)
            {
                throw new JsonException("Response has unknown format");
            }

            return responseContent;
        }
        #endregion

    }
}
