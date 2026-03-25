using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class Attachment
{
    public int Id { get; set; }

    public int? CallLogId { get; set; }

    public int? UpdateId { get; set; }

    public string FileName { get; set; } = null!;

    public string? FileType { get; set; }

    public decimal? FileSizeKb { get; set; }

    public string FileStoragePath { get; set; } = null!;

    public int UploadedById { get; set; }

    public DateTime UploadedAt { get; set; }

    public bool IsClientUpload { get; set; }

    public virtual Log? CallLog { get; set; }

    public virtual LogUpdate? Update { get; set; }

    public virtual Person UploadedBy { get; set; } = null!;
}
