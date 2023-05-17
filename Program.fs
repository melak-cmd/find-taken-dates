// A program to find taken dates for pictures

// Import the System.IO and System.Drawing namespaces
open System.IO
open System.Drawing
open System
open System.Collections.Generic

// Define a function to get the taken date of a picture file
let getTakenDate (filename: string) =
  // Try to open the file as an image
  try
    use image = Image.FromFile(filename)
    // Try to get the date taken property from the image metadata
    try
      let dateTaken = image.GetPropertyItem(36867) // PropertyTagExifDTOrig
      // Convert the date taken bytes to a string
      let dateTakenStr = System.Text.Encoding.ASCII.GetString(dateTaken.Value)      
      // Parse the date taken string to a DateTime object
      let dateTakenDt = System.DateTime.ParseExact(dateTakenStr, "yyyy:MM:dd HH:mm:ss\0", null)
      // Return the date taken as a string in ISO format
      Some (dateTakenDt.ToString("yyyy-MM-dd"))
    with
      // If the date taken property is not found, return None
      | :? KeyNotFoundException -> None
  with
    // If the file is not an image, return None
    | :? OutOfMemoryException -> None

// Define a function to find all picture files in a directory and its subdirectories
let findPictureFiles (dir: string) =
  // Get all files in the directory and its subdirectories
  let files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
  // Filter the files by their extensions
  let pictureFiles = files |> Array.filter (fun file ->
    let ext = Path.GetExtension(file).ToLower()
    ext = ".jpg" || ext = ".jpeg" || ext = ".png" || ext = ".bmp" || ext = ".gif")
  // Return the picture files as an array
  pictureFiles

// Define a function to print the taken dates for all picture files in a directory and its subdirectories
let printTakenDates (dir: string) =
  // Find all picture files in the directory and its subdirectories
  let pictureFiles = findPictureFiles dir
  // For each picture file, get its taken date and print it
  for file in pictureFiles do
    let takenDate = getTakenDate file
    match takenDate with
    | Some date -> printfn "%s was taken on %s" file date
    | None -> printfn "%s has no taken date" file

// Ask the user for a directory to scan for picture files
printfn "Enter a directory to scan for picture files:"
let dir = Console.ReadLine()

// Print the taken dates for all picture files in the directory and its subdirectories
printTakenDates dir

