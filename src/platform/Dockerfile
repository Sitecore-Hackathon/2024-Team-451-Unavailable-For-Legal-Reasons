# escape=`

ARG PARENT_IMAGE
ARG TOOLS_IMAGE
ARG MANAGEMENT_SERVICES_IMAGE
ARG HEADLESS_SERVICES_IMAGE
ARG DELTA_ASSET_IMAGE

FROM ${TOOLS_IMAGE} as tools
FROM ${MANAGEMENT_SERVICES_IMAGE} AS management_services
FROM ${HEADLESS_SERVICES_IMAGE} AS headless_services
FROM ${DELTA_ASSET_IMAGE} as deltaAssetImage

# ---
FROM ${PARENT_IMAGE} AS downloads
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR C:/out/downloads
RUN curl.exe -sS -L -o '.\urlrewrite.msi' https://download.microsoft.com/download/1/2/8/128E2E22-C1B9-44A4-BE2A-5859ED1D4592/rewrite_amd64_en-US.msi;

# ---
FROM ${PARENT_IMAGE}
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# install IIS UrlRewrite
COPY --from=downloads C:/out/downloads/ C:/downloads
RUN Start-Process -Wait -FilePath msiexec -ArgumentList '/i', 'C:\\downloads\\urlrewrite.msi', '/quiet', '/norestart' -NoNewWindow; `
    Remove-Item 'C:\\downloads' -Force -Recurse

# copy developer tools and entrypoint
COPY --from=tools C:\tools C:\tools

WORKDIR C:\inetpub\wwwroot

# copy the Sitecore Management Services Module
COPY --from=management_services C:\module\cm\content C:\inetpub\wwwroot

# Copy and init the JSS / Headless Services Module
COPY --from=headless_services C:\module\cm\content C:\inetpub\wwwroot
COPY --from=headless_services C:\module\tools C:\module\tools
RUN C:\module\tools\Initialize-Content.ps1 -TargetPath C:\inetpub\wwwroot; `
    Remove-Item -Path C:\module -Recurse -Force;

# copy published web project
COPY . .

# apply transformations
RUN & 'C:\\tools\\scripts\\Invoke-XdtTransform.ps1' -Path 'C:\\inetpub\\wwwroot\\Web.config' -XdtPath 'C:\\inetpub\\wwwroot\\Web.Common.config.xdt'; `
    Remove-Item -Path 'C:\\inetpub\\wwwroot\\*.xdt' -Force;

# copy Sitecore hotfixes
COPY --from=deltaAssetImage /platform/cm/inetpub/wwwroot/ ./
