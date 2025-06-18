# Electronic Store Frontend

Frontend React application for the Electronic Store with admin panel and user features.

## Features

- 🛍️ **Product Management**: Browse, search, and filter products
- 🛒 **Shopping Cart**: Add products to cart and manage quantities
- 👤 **User Authentication**: Login, register, and profile management
- 💳 **Wallet System**: Manage user balance and transactions
- 📦 **Order Management**: View and manage orders
- 🎛️ **Admin Panel**: Complete admin dashboard for managing users, products, orders, and wallets
- 📸 **Image Upload**: Upload product images using Cloudinary

## Technologies Used

- **React 18** - UI framework
- **Vite** - Build tool
- **Zustand** - State management
- **React Router** - Routing
- **React Hook Form** - Form handling
- **Axios** - HTTP client
- **Sonner** - Toast notifications
- **Lucide React** - Icons
- **Tailwind CSS** - Styling

## Setup Instructions

### 1. Install Dependencies

```bash
npm install
```

### 2. Environment Configuration

Create a `.env` file in the frontend directory:

```env
# Cloudinary Configuration
VITE_CLOUDINARY_NAME=your_cloudinary_cloud_name
VITE_CLOUDINARY_UPLOAD_PRESET=your_upload_preset

# API Configuration
VITE_API_URL=http://localhost:5000
```

### 3. Cloudinary Setup

1. Sign up for a free Cloudinary account at [cloudinary.com](https://cloudinary.com)
2. Get your Cloud Name from the dashboard
3. Create an upload preset:
   - Go to Settings > Upload
   - Scroll to Upload presets
   - Create a new preset or use the default "ml_default"
   - Set signing mode to "unsigned" for client-side uploads

### 4. Start Development Server

```bash
npm run dev
```

The application will be available at `http://localhost:5173`

## Project Structure

```
src/
├── components/          # Reusable components
│   ├── auth/           # Authentication components
│   └── layout/         # Layout components
├── pages/              # Page components
│   ├── admin/          # Admin pages
│   └── auth/           # Authentication pages
├── services/           # API services
├── stores/             # Zustand stores
└── App.jsx             # Main app component
```

## Features Overview

### User Features
- **Home**: Welcome page with featured products
- **Products**: Browse and search products with filters
- **Product Detail**: View detailed product information
- **Cart**: Manage shopping cart
- **Orders**: View order history
- **Profile**: Manage account information and wallet

### Admin Features
- **Dashboard**: Overview of system statistics
- **Users**: Manage user accounts and roles
- **Products**: Create, edit, and delete products with image upload
- **Orders**: View and manage all orders
- **Wallet**: Manage user balances

## API Integration

The frontend integrates with the backend API endpoints:

- **Authentication**: `/api/User/*`
- **Products**: `/api/Products/*`
- **Cart**: `/api/Cart/*`
- **Orders**: `/api/Order/*`
- **Wallet**: `/api/Wallet/*`

## Image Upload

Product images are uploaded to Cloudinary using the `uploadImage` service:

```javascript
import { uploadImage } from '../services/uploadImage';

// Upload image
const formData = new FormData();
formData.append('file', imageFile);
formData.append('upload_preset', 'your_preset');

const imageUrl = await uploadImage(formData);
```

## State Management

The application uses Zustand for state management with three main stores:

- **authStore**: User authentication and profile
- **productStore**: Product data and admin operations
- **cartStore**: Shopping cart management

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.
