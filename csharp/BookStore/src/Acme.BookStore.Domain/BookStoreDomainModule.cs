using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Acme.BookStore.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using System;
using Microsoft.Extensions.Configuration;
using Acme.BookStore.Books;

namespace Acme.BookStore;

[DependsOn(
    typeof(BookStoreDomainSharedModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpTenantManagementDomainModule),
    typeof(AbpEmailingModule),
    typeof(AbpBlobStoringMinioModule)
)]
public class BookStoreDomainModule : AbpModule
{
    private IConfiguration _configuration { get; set; }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        _configuration = context.Services.GetConfiguration();

        ConfigureMinioStorage();

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية", "ae"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English", "gb"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("hr", "hr", "Croatian"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish", "fi"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français", "fr"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский", "ru"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak", "sk"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe", "tr"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español"));
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }

    private void ConfigureMinioStorage()
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseMinio(minio =>
                {
                    minio.EndPoint = "localhost:9000";
                    minio.AccessKey = "minioadmin";
                    minio.SecretKey = "minioadmin";
                    minio.BucketName = "bookStore";
                    minio.WithSSL = false;
                    minio.CreateBucketIfNotExists = true;
                });
            });

            ConfigureMinioContainer<ProfilePictureContainer>(options);
        });
    }

    private void ConfigureMinioContainer<T>(AbpBlobStoringOptions options)
    {
        options.Containers.Configure<T>(containerConfiguration =>
        {
            containerConfiguration.UseMinio(x =>
            {
                x.BucketName = $"ebpay-{GetContainerName<T>()}";
                x.EndPoint = "localhost:9000";
                x.AccessKey = "minioadmin";
                x.SecretKey = "minioadmin";
                x.BucketName = "bookStore";
                x.WithSSL = false;
                x.CreateBucketIfNotExists = true;
            });
        });
    }

    private static string GetContainerName<T>()
    {
        var t = typeof(T);
        var attribute = (BlobContainerNameAttribute) Attribute.GetCustomAttribute(t, typeof (BlobContainerNameAttribute));
        return attribute == null ? t.Namespace : attribute.Name;
    }
}
