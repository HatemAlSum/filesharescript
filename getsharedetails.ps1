$ResourceGroupName="YOUR-RESOURCE-GROUP"
$StorageAccountName ="YOUR-STORAGE-ACCOUNT"
$FileShareName ="YOUR-FILE-SHARE"


# Function to convert bytes to human-readable format
function Convert-Size {
    param([long]$size)
    $sizes = 'Bytes,KB,MB,GB,TB'
    $sizes = $sizes.Split(',')
    $index = 0
    while ($size -ge 1kb -and $index -lt ($sizes.Count - 1)) {
        $size = $size / 1kb
        $index++
    }
    return "{0:N2} {1}" -f $size, $sizes[$index]
}

# Get storage account context
$storageAccount = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName
$ctx = $storageAccount.Context

# Get file share
$share = Get-AzStorageShare -Name $FileShareName -Context $ctx

Write-Host "Analyzing folders in file share '$FileShareName'...`n"
Write-Host "Folder Structure and Sizes:"
Write-Host "----------------------------------------"



$folders = Get-AzStorageFile -ShareName $FileShareName -Path $path -Context $context

foreach ($folder in $folders) {         
    if ($folder.GetType().Name -eq "AzureStorageFileDirectory") {

        $foldersize=0
        $files = $folder.CloudFileDirectory.ListFilesAndDirectories()
        foreach($file in $files)
        {
                $foldersize += $file.Properties.Length

        }
        Write-Host "$($folder.Name)`t$(Convert-Size $foldersize)"
    }
}


