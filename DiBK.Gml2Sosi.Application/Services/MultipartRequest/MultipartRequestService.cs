﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace DiBK.Gml2Sosi.Application.Services.MultipartRequest
{
    public class MultipartRequestService : IMultipartRequestService
    {
        private static readonly Regex _gml32Regex = 
            new(@"^<\?xml.*?<\w+:FeatureCollection.*?xmlns:\w+=""http:\/\/www\.opengis\.net\/gml\/3\.2""", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MultipartRequestService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IFormFile> GetFileFromMultipartAsync()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var reader = new MultipartReader(request.GetMultipartBoundary(), request.Body);
            MultipartSection section;

            try
            {
                while ((section = await reader.ReadNextSectionAsync()) != null)
                {
                    if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition) &&
                        contentDisposition.IsFileDisposition() &&
                        contentDisposition.Name.Value == "gmlFile" &&
                        await IsGml32File(section))
                    {
                        return await CreateFormFile(contentDisposition, section);
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private static async Task<IFormFile> CreateFormFile(ContentDispositionHeaderValue contentDisposition, MultipartSection section)
        {
            var memoryStream = new MemoryStream();
            await section.Body.CopyToAsync(memoryStream);
            await section.Body.DisposeAsync();
            memoryStream.Position = 0;

            return new FormFile(memoryStream, 0, memoryStream.Length, contentDisposition.Name.ToString(), contentDisposition.FileName.ToString())
            {
                Headers = new HeaderDictionary(),
                ContentType = section.ContentType
            };
        }

        private static async Task<bool> IsGml32File(MultipartSection section)
        {
            var buffer = new byte[1000];
            await section.Body.ReadAsync(buffer.AsMemory(0, 1000));
            section.Body.Position = 0;

            using var memoryStream = new MemoryStream(buffer);
            using var streamReader = new StreamReader(memoryStream);
            var gmlString = streamReader.ReadToEnd();

            return _gml32Regex.IsMatch(gmlString);
        }
    }
}
