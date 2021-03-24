using System;

namespace Data.Models.Interfaces
{
    public interface IBaseModel
    {
        int ID { get; set; }

        DateTime CreateTime { get; set; }
        int CreateUserId { get; set; }

        DateTime? UpdateTime { get; set; }
        int? UpdateUserId { get; set; }

        string ToJson();
    }
}