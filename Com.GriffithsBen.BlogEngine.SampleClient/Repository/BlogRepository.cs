﻿using Com.GriffithsBen.BlogEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GriffithsBen.BlogEngine.SampleClient.Repository {

    /// <summary>
    /// A mock repository for illustration purposes
    /// </summary>
    public class BlogRepository {

        private List<BlogEntry> BlogEntries { get; set; }

        public BlogRepository() {
            // initialise with a couple of dummy blog entries
            this.BlogEntries = new List<BlogEntry>() {

                new BlogEntry() {
                    Content = "[p][b]This[/b] is the first dummy blog entry[/p][p]It consists of two paragraphs[/p]",
                    Date = DateTime.Now,
                    DisplayName = "Dummy Entry 1",
                    Id = 1,
                    Title = "Dummy Entry 1"
                },
                new BlogEntry() {
                    Content = @"[p][b]This[/b] is the [i]second[/i] dummy blog entry[/p][p]It consists of [i]three[/i] paragraphs[/p]
                                [p]I repeat, it consists of [i]three[/i] paragraphs[/p]",
                    Date = DateTime.Now,
                    DisplayName = "Dummy Entry 2",
                    Id = 2,
                    Title = "Dummy Entry 2"
                }

            };
        }

        public IEnumerable<BlogEntry> GetBlogEntries() {
            return this.BlogEntries;
        }

        public BlogEntry GetBlogEntryById(int id) {
            return this.BlogEntries.Where(x => x.Id == id).SingleOrDefault();
        }

        public void InsertBlogEntry(BlogEntry entry) {
            this.BlogEntries.Add(entry);
        }

        public void UpdateBlogEntry(BlogEntry entry) {
            BlogEntry savedEntry = this.BlogEntries.Find(x => x.Id == entry.Id);
            savedEntry.Comments = entry.Comments;
            savedEntry.Content = entry.Content;
            savedEntry.Date = entry.Date;
            savedEntry.DisplayName = entry.DisplayName;
            savedEntry.SynopsisLength = entry.SynopsisLength;
            savedEntry.Title = entry.Title;
        }

        public void DeleteBlogEntry(int id) {
            BlogEntry savedEntry = this.BlogEntries.Find(x => x.Id == id);
            this.BlogEntries.Remove(savedEntry);
        }

    }
}