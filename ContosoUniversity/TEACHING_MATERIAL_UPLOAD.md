# Teaching Material Image Uploads

Course create and edit pages accept JPG, JPEG, PNG, GIF, and BMP images up to 5 MB. Files receive generated names and are stored under `Uploads/TeachingMaterials`, outside `wwwroot`.

Images are served through an authenticated controller endpoint. Replacing an image removes the previous file after the new upload succeeds; deleting a course removes its associated image.

The app must have write access to the upload directory. In production, persist and back up this directory separately from the application deployment. Uploaded files are excluded from source control; the `.gitkeep` file preserves the directory structure.
