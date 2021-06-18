using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

public enum DeviceStatus { Online, Offline }

public class Gateway
{
    [Key]
    public string SerialNumber { get; set; }
    public string Name { get; set; }

    public string IP { get; set; }

    public ICollection<Device> PeripheralDevices { get; set; } = new List<Device>();
}

public class Device
{
    [Key]
    public long Guid { get; set; }
    public long UID { get; set; }
    public string Vendor { get; set; }
    public DateTime DateCreated { get; set; }
    public DeviceStatus Status{ get; set; }

}