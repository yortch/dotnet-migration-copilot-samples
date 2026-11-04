# Teaching Material Image Uploads

Courses can have an optional teaching material image. The create and edit forms accept JPG, JPEG, PNG, GIF, and BMP files up to 5 MB.

Uploaded images are written to `wwwroot/Uploads/TeachingMaterials` and served as static files. New filenames include the course ID and a GUID. Replacing an image removes the previous local file; deleting a course also attempts to remove its image.

Uploaded files are excluded from source control. The storage is local to the application instance, so production deployments should use persistent or cloud-backed storage and configure suitable upload limits and write permissions.

The application does not currently configure authentication or authorization for uploads.
