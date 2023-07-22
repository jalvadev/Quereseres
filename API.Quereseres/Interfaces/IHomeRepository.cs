﻿using API.Quereseres.Models;

namespace API.Quereseres.Interfaces
{
    public interface IHomeRepository : IDisposable
    {
        public Home InsertHome(Home home);

        public Home GetHomeByIdAndUser(int homeId, User user);

        void Save();
    }
}