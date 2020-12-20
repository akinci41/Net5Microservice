using Feeder.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Feeder.Utils
{
    public class Reporter
    {
        public static void GenerateReportAsync(Guid guid)
        {
            Thread thread = new Thread(() => GenerateReport(guid));
            thread.Start();
        }

        private static void GenerateReport(Guid guid)
        {
            Thread.Sleep(5000); //sleep ten seconds to see pending status :)

            var context = new FeederContext();
            var report = context.Reports.Where(x => x.ID == guid).FirstOrDefault();
            report.Status = (int)ReportStatus.Preparing;
            context.Reports.Attach(report).Property(x => x.Status).IsModified = true;
            context.SaveChanges();

            Thread.Sleep(5000); //sleep ten seconds to see preparing status :)

            var distinctLocation = context.Communications.Where(x => x.Type == "3").GroupBy(x => x.Content).Select(x => x.Key).ToList(); //get the distinct location list
            var list = new List<ReportDetail>();

            //iterate list to create report
            foreach (var location in distinctLocation)
            {
                var detail = new ReportDetail();
                detail.ReportID = guid;
                detail.Location = location;

                //one contact may have different location info, therefore we must eliminate recurrence
                var distinctContactList = context.Communications.Where(x => x.Type == "3" && x.Content == location).Select(y => y.ContactID).Distinct().ToList(); //distinct contact list in current location
                detail.ContactCount = distinctContactList.Count;

                var distinctPhoneList = context.Communications.Where(x => x.Type == "2" && distinctContactList.Contains(x.ContactID)).Select(y => y.ID).Distinct().ToList(); //distinct phone ID list 
                detail.PhoneCount = distinctPhoneList.Count;
                list.Add(detail);
            }

            context.ReportDetails.AddRange(list);
            context.SaveChanges();

            report.Status = (int)ReportStatus.Ready;
            context.Reports.Attach(report).Property(x => x.Status).IsModified = true;
            context.SaveChanges();
        }
    }
}
