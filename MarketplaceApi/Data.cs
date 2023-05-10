using System;

namespace MarketplaceApi
{
    public class Data
    {
        //All tables
        public int[] Id = { 1, 2, 3, 4, 5 };

        //Users data
        public string[] UsersPhones = { "+79103455432", "+79156455633", "+79166457663", "+79102457063", "+79152455069" };
        public string[] UsersEmails = { "aaa@mail.com", "bbb@mail.com", "ccc@mail.com", "ddd@mail.com", "eee@mail.com" };
        public string[] UsersFirstNames = { "Daniil", "Boris", "Genadiy", "Natalya", "Alena" };
        public string[] UsersSecondNames = { "aaa", "bbb", "ccc", "ddd", "eee" };
        public DateTime[] UsersRegistartionDate = new DateTime[]
        {
            new DateTime(2019, 5, 21),
            new DateTime(2020, 3, 14),
            new DateTime(2020, 8, 6),
            new DateTime(2020, 12, 3),
            new DateTime(2021, 2, 12),
        };

        public string[] UsersDeliveryAdresses = {"Moscow", "St Petersburg", "Kazan'", "Volgograd", "Ufa" };
        
        //Products data
        public int[] ProductsUserIds = { 4, 1, 4, 5, 5 };
        public string[] ProductsNames =
            { "ProductName1", "ProductName2", "ProductName3", "ProductName4", "ProductName5" };

        public string[] ProductsMaterials = { "Material1", "Material2", "Material1", "Material3", "Material2" };
        public int[] ProductsLenths = {4, 10, 15, 3, 17 };
        public int[] ProductsWidth = {3, 7, 12, 2, 14 };
        public int[] ProductsHighs = {12, 6, 18, 11, 6 };
        public int[] ProductsPrices = { 2000, 3000, 1500, 700, 3500 };
        public int[] ProductsQuantities = { 20, 30, 25, 10, 8 };
        public DateTime[] ProductsPublicationDates = new DateTime[]
        {
            new DateTime(2020, 6, 29),
            new DateTime(2020, 7, 13),
            new DateTime(2020, 11, 21),
            new DateTime(2020, 12, 29),
            new DateTime(2021, 4, 3),
        };

        //Orders data
        public DateTime[] OrdersOrderDates = new DateTime[]
        {
            new DateTime(2020, 7, 28),
            new DateTime(2020, 8, 12),
            new DateTime(2020, 9, 20),
            new DateTime(2021, 1, 28),
            new DateTime(2021, 5, 2),
        };
        public DateTime[] OrdersReceiveDates = new DateTime[]
        {
            new DateTime(2020, 6, 30),
            new DateTime(2020, 7, 14),
            new DateTime(2020, 11, 22),
            new DateTime(2020, 12, 30),
            new DateTime(2021, 4, 4),
        };
        public string[] OrdersOrderStatus = {"In stock", "On the way", "Delivered",  "On the way", "In stock"};
        public int[] OrdersUserIds = { 2, 3, 2, 1, 3 };
        
        //bills data
        public DateTime[] BillsSellDate = new DateTime[]
        {
            new DateTime(2020, 7, 28),
            new DateTime(2020, 8, 12),
            new DateTime(2020, 9, 20),
            new DateTime(2021, 1, 28),
            new DateTime(2021, 5, 2),
        };
        public string[] BillsWayOfPayement = { "card", "cash", "cash", "card", "card" };
        public int[] BillsUserIds = { 2, 3, 2, 1, 3 };
    }
}