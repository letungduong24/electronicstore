import axios from "axios";

export const uploadImage = async (image) => {
    try {
        const response = await axios.post(
            `https://api.cloudinary.com/v1_1/${import.meta.env.VITE_CLOUDINARY_NAME || 'demo'}/upload`,
            image,
            {
              headers: { "Content-Type": "multipart/form-data" },
            }
          );
          if (response.data.secure_url) {
            return response.data.secure_url
          } else {
            return null
          }
    } catch (error) {
        console.error('Error uploading image:', error);
        return null;
    }
}