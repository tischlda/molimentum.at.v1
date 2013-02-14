Get-ChildItem -Recurse -Filter "App.Sample.config" | `
ForEach-Object {
    $new_config = $_.Fullname.Replace(".Sample.", ".")
    if (!(Test-Path $new_config))
    {
        Copy-Item $_.FullName $new_config
    }
}
Copy-Item "Molimentum.Web\Web.Sample.Config" "Molimentum.Web\Web.Debug.Config"
Copy-Item "Molimentum.Web\Web.Sample.Config" "Molimentum.Web\Web.Release.Config"