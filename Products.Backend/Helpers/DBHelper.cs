﻿namespace Products.Backend.Helpers
{
    using Models;
    using System;
    using System.Threading.Tasks;

    public class DBHelper
    {
        public static async Task<Response> SaveChangesAsync(DataContextLocal db)
        {
            try
            {
                await db.SaveChangesAsync();
                return new Response { Succeeded = true, };
            }
            catch (Exception ex)
            {
                var response = new Response { Succeeded = false, };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "There is a record with the same value";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "The record can't be delete because it has related records";
                }
                else
                {
                    response.Message = ex.Message;
                }

                return response;
            }
        }
    }
}