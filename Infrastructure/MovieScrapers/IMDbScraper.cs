﻿using Domain.Abstractions;
using Domain.Entities;
using HtmlAgilityPack;
using Infrastructure.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MovieScrapers
{
    [Scraper("IMDb", "https://www.imdb.com")]
    internal class IMDbScraper : ScraperBase
    {
        public IMDbScraper(ILogger<IMDbScraper> logger,  IHttpClientFactory httpClientFactory, IActorRepository actorRepository) :
            base(logger ,httpClientFactory, actorRepository)
        { }

        public override async Task ScrapeActorsAsync()
        {
            var url = "/list/ls054840033/";
            var html = await _httpClient.GetStringAsync(url);
            var actors = ParseActors(html);
            await _actorRepository.AddActorsAsync(actors);
        }

        private IEnumerable<Actor> ParseActors(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var actors = new List<Actor>();

            var actorNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='lister-item mode-detail']");

            if (actorNodes != null)
            {
                foreach (var node in actorNodes)
                {
                    var actor = ParseActor(node);
                    if (actor != null)
                    {
                        actors.Add(actor);
                    }
                }
            }

            return actors;
        }

        private Actor? ParseActor(HtmlNode node)
        {
            try
            {
                var nameNode = node.SelectSingleNode(".//h3/a");
                var typeNode = node.SelectSingleNode(".//p[@class='text-muted text-small']");
                var detailsNode = node.SelectSingleNode(".//div[@class='list-description']/p");
                var rankNode = node.SelectSingleNode(".//span[@class='lister-item-index unbold text-primary']");

                string? name = nameNode?.InnerText.Trim();
                int? rank = ParseRank(rankNode?.InnerText);

                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogWarning("Skipping actor due to missing name");
                    return null;
                }

                if (rank == null)
                {
                    _logger.LogWarning("Skipping actor due to missing  rank");
                    return null;
                }

                return new Actor
                {
                    Name = name,
                    Details = WebUtility.HtmlDecode(detailsNode?.InnerText.Trim()),
                    Type = ExtractType(typeNode?.InnerText.Trim()),
                    Rank = rank.Value,
                    Source = "IMDb"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing actor node");
                return null;
            }
        }

        private string ExtractType(string rawType)
        {
            if (!string.IsNullOrEmpty(rawType))
            {
                var parts = rawType.Split('|');
                if (parts.Length > 0)
                {
                    return parts[0].Trim();
                }
            }
            return "Unknown";
        }

        private int? ParseRank(string rankText)
        {
            if (!string.IsNullOrEmpty(rankText))
            {
                var rank = rankText.Trim().Split('.')[0];
                if (int.TryParse(rank, out int result))
                {
                    return result;
                }
            }
            return null; 
        }
    }
}
