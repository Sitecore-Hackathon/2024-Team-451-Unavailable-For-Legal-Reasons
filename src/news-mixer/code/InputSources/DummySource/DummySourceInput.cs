﻿using NewsMixer.Models;
using System.Runtime.CompilerServices;

namespace NewsMixer.InputSources.DummySource
{
    internal class DummySourceInput : ISourceInput
    {
        public async IAsyncEnumerable<NewsItem> Execute([EnumeratorCancellation]CancellationToken token)
        {
            await Task.Delay(0);

            yield return new NewsItem()
            {
                Date = DateTime.Now,
                Title = "Scaling the experience with the multi-brand Catalyst and Sitecore XM",
                Content = "Sitecore has released new Experience Manager SaaS-based CMS, XM Cloud. Now, with our new Valtech Catalyst, multi-brand organizations can deploy their brand sites on XM Cloud in less time, reducing cost, improving quality, and increasing consistency of the site deployment.\r\n\r\n \r\n\r\nWebsite deployments for large brands with hundreds of sites can typically take 50+ days - per brand site - to rollout. We're seeing that time reduced by up to 40% thanks to automated deployment of brand and partner websites on Sitecore XM Cloud. That’s the power of the Valtech Catalyst.\r\n\r\nUna Verhoeven\r\n\r\n\r\n \r\nVP Global Technology - Sitecore, Valtech\r\n \r\n\r\nGreat creative at scale: what is the Valtech Catalyst, and how does it work?\r\n \r\n\r\nMulti-brand companies who create, deploy and update content across multiple brand-based websites know that it can be complex and time consuming to make changes across their ecosystem. In many cases, design systems can help to increase efficiency and relieve some of the pressure on their teams and resources. But, when Sitecore released XM Cloud, we saw an amazing opportunity to transform the multi-brand site roll out challenge, for good. \r\n \r\nIn conjunction with new Sitecore XM Cloud, the Valtech Catalyst supercharges your design system and gives you the power to elevate your customer experience quickly, connecting front end automation with creative systems, to apply themes efficiently and consistently across multiple brand and partner sites. Containing the templates, design system, creative and content which can be pulled into a user experience, the Valtech Catalyst creates centralized and localized content structures depending on the organization’s brand and content management strategy. These templates are created based on the business goals of the websites through a review and prioritization of the required functionality. \r\n\r\nWith our Catalyst - built specifically for Sitecore XM Cloud customers - we've made it possible to automate the roll out of your brand sites; reducing time, cost and complexity, and giving you more time to concentrate on delivering the experience your customers need. \r\n\r\n \r\n\r\nPlease accept marketing cookies to watch this video.\r\nThe Perfect Blend of Technology, Content, and Design\r\n \r\n\r\nThe Valtech Catalyst combines the technology, content and design elements that will enable you to scale your ambitions in much shorter timescales. \r\n\r\nIn fact, with our automated deployment system, we are already enabling brands to deploy more quickly and consistently on to Sitecore XM Cloud, reducing site rollout times by up to 40%.  \r\n\r\nHere's an outline of a typical feature request flow:\r\n\r\nValtech-Catalyst-Landing-Page-Body-Image-1.jpg\r\n\r\nAdding automated brand styling, component libraries, categorized site templates, built-in quality control, and dependable processes to automate the creation of consistent world-class experiences, the Valtech Catalyst has an established content architecture that is well-organized and contains global content, multi-language category templates, data sources, URL structures, media items and configuration that can be shared across brand sites per the business requirements.  \r\n \r\nNow let's see the impact the Valtech Catalyst can make: \r\n\r\nValtech-Catalyst-Landing-Page-Body-Image-2.jpg\r\n\r\nThe deployment process uses an automated and efficient end-to-end CI/CD pipeline, leveraging the best DevOps tools. Redirects are managed, KPIs are established and tracked from pre-built dashboards, and go-live is managed from the approved timeline. \r\n\r\nWe're providing the consistency and innovation needed to help organizations reduce time to market, be more efficient, and better visualize and operationalize the overall deployment process onto Sitecore XM Cloud though with the Valtech Catalyst. \r\n\r\n \r\n\r\n\r\n\r\n\r\n\r\nThe ultimate multi-brand story: discover the L'Oréal website factory\r\n \r\n\r\nImplementing a new way of working, Anne Guichard shares her experiences from delivering L'Oréal's Website Factory. Responsible for the entire journey, Anne has unique insights from both the technical and the organizational perspective - all of them shared in this podcast. Listen the podcast\r\n\r\n\r\n \r\nThe latest innovation from Valtech & Sitecore\r\n\r\n\r\nThe Valtech Catalyst  is just the latest innovation from Valtech & Sitecore following more than two decades of trusted partnership.\r\n\r\nAs a Sitecore Platinum Solutions Partner, we have already completed more than 3000+ Sitecore deployments across the world, and were the first Sitecore partner globally to earn all product specializations (Experience Platform, Content Hub, CDP and Personalize and OrderCloud). In 2022, Valtech was awarded with the Customer Value and Impact Prize and Delivery Excellence Prize at the Sitecore Partner Awards, and it’s this close partnership, and a never-ending focus on the customer experience, that makes working with Valtech and Sitecore so exciting. \r\n\r\n\r\n \r\nIf you'd like to hear more about the Valtech Catalyst, fill out the form below to get in touch with the team today.",
                Categories = ["Sitecore XM"],
                Url = new Uri("https://www.valtech.com/en-dk/blog/scaling-the-experience-with-the-valtech-catalyst-and-sitecore-xm-cloud/"),
                OriginalLanguage = "en",
                ContentLanguage = "en",
                ImageUrl = new Uri("https://www.valtech.com/4af122/globalassets/00-global/02-images/04-insights/scaling-the-experience-with-the-multi-brand-catalyst-and-sitecore-xm/2023/valtech-catalyst-landing-page-header-navigation.jpg?width=870&height=576&mode=crop&format=jpg"),
            };
        }
    }
}
