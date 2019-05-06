﻿using MeetingInfoDatabase;
using MeetingInfoDatabase.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MeetingInfoDatabaseTest
{

    [TestClass]
    public class UnitTestDataSet
    {
        private Repository repository;

        [TestInitialize]
        public void TestInitialize()
        {
            repository = new Repository(TestConfiguration.ConnectionString, MeetingInfoDatabase.DAO.DatabaseType.DataSet);
        }

        [TestMethod]
        public void TestClientGet()
        {
            Assert.IsTrue(repository.GetClientTable().GetClients().Count > 0);
        }

        [TestMethod]
        public void TestMeetingsGetInvalid()
        {
            Assert.IsFalse(repository.GetMeetingsTable().GetMeetings(TestConfiguration.InvalidDateTime).Any());
        }

        [TestMethod]
        public void TestMeetingsGetValid()
        {
            Assert.IsTrue(repository.GetMeetingsTable().GetMeetings(TestConfiguration.ValidDateTime).Any());
        }

        [TestMethod]
        public void TestMeetingsAddChangeRemoveMeeting()
        {
            Meeting meeting = TestConfiguration.ValidMeeting;
            Assert.IsTrue(repository.GetMeetingsTable().AddMeeting(meeting));
            Console.WriteLine(meeting.IDMeeting);
            Assert.AreEqual(meeting.Title, repository.GetMeetingsTable().GetMeeting(meeting.IDMeeting).Title);

            meeting.Title = TestConfiguration.UpdatedTitle;
            Assert.IsTrue(repository.GetMeetingsTable().ChangeMeeting(meeting));
            Assert.AreEqual(meeting.Title, repository.GetMeetingsTable().GetMeeting(meeting.IDMeeting).Title);

            Assert.IsTrue(repository.GetMeetingsTable().RemoveMeeting(meeting));
            Assert.IsNull(repository.GetMeetingsTable().GetMeeting(meeting.IDMeeting));
        }

    }
}
