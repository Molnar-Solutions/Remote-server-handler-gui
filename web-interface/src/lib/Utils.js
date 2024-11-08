export function getFileExtension(url) {
    // Split the URL by the last forward slash
    const parts = url.split('/');

    // Get the filename (last element)
    const filename = parts[parts.length - 1];

    // Check for a period indicating extension
    const dotIndex = filename.lastIndexOf('.');
    if (dotIndex !== -1) {
        // Extract the extension (everything after the last dot)
        return filename.slice(dotIndex + 1);
    } else {
        // No extension found, return an empty string or specific message
        return ''; // Or a message like "No extension found"
    }
}