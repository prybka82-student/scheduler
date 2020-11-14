using System;
using System.Threading.Tasks;
using Scheduler.App.Entities;
using Scheduler.App.Interfaces;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.IO;
using Scheduler.App.Extensions;
using System.ComponentModel.Design;
using System.Threading;
using System.Linq;

namespace Scheduler.Impl.Mailer
{
    public class Mailer : IMailer
    {
        string _template = "";

        string _deliveryDirectory = "";

        object _model = new { Title = "Sir/Madam", Surname = "Smith", Discount = 0.4M };

        public Mailer(string template, string deliveryDirectory)
        {
            _template = template ?? "<h1>Dear @Model.Title @Model.Surname</h1><p>We would like to offer you a discount of @Model.Discount.</p><p>Please, feel free to use it at your convenience.</p><p>Yours Best Vendor</p>";
            _deliveryDirectory = deliveryDirectory ?? "..\\emails";

            CreateDeliveryDirectory(_deliveryDirectory);

            FluentEmail.Core.Email.DefaultSender = SmtpSenderFactory(_deliveryDirectory);
        }

        public ActionResult<App.Entities.Email> Send(App.Entities.Email email, CancellationToken cancellationToken, ILogger logger)
        {
            var emailToSend = MailFactory(email, logger);            

            if (emailToSend.Send(cancellationToken).Successful)
                return new ActionResult<App.Entities.Email>(ResultType.OK, email.ToSingleItemSequence().ToList());
            else
                return new ActionResult<App.Entities.Email>(ResultType.error, email.ToSingleItemSequence().ToList());
        }

        public async Task<ActionResult<App.Entities.Email>> SendAsync(App.Entities.Email email, CancellationToken cancellationToken, ILogger logger)
        {
            var emailToSend = MailFactory(email, logger);

            if ((await emailToSend.SendAsync(cancellationToken)).Successful)
                return new ActionResult<App.Entities.Email>(ResultType.OK, email.ToSingleItemSequence().ToList());
            else
                return new ActionResult<App.Entities.Email>(ResultType.error, email.ToSingleItemSequence().ToList());
        }

        private object EmailToModel(Scheduler.App.Entities.Email email)
        {
            return new
            {
                Title = email.To.Title,
                Surname = email.To.Surname,
                Discount = email.Content
            };
        }

        private SmtpSender SmtpSenderFactory(string deliveryDirectory, string host = "localhost", bool enableSSl = false)
        {
            return new SmtpSender(() => new SmtpClient(host)
            {
                EnableSsl = enableSSl,
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = deliveryDirectory
            });
        }

        private void CreateDeliveryDirectory(string directory)
        {
            if (Directory.Exists(_deliveryDirectory).No()) Directory.CreateDirectory(_deliveryDirectory);
        }

        private IFluentEmail MailFactory(Scheduler.App.Entities.Email email, ILogger logger)
        {
            try
            {
                logger.Debug("Creating new e-mail...");

                return FluentEmail.Core.Email
                    .From(emailAddress: email.From.Email, name: $"{email.From.Name} {email.From.Surname}")
                    .To(emailAddress: email.To.Email, name: $"{email.To.Name} {email.To.Surname}")
                    .Subject(email.Subject)
                    .UsingTemplate(_template, EmailToModel(email));
            }
            catch (Exception e)
            {
                logger.Exception(e);
                throw e;
            }            
        }

    }
}
