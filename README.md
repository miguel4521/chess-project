# Chess Project

A web-based chess application built with .NET 9.0 and React. This project is abandoned.

<img width="951" alt="image" src="https://github.com/user-attachments/assets/854d4d69-4a10-4c1d-89d2-cf2e54b12ec1" />

## ⚠️ Development Status

Please note:
- Move generation and validation may not be 100% accurate
- AI/computer opponent features are still being refined
- Some chess rules might not be fully implemented

## 🚀 Tech Stack

- Backend: .NET 9.0
- Frontend: React
- Development Environment: Node.js

## 🛠️ Setup

### Prerequisites
- .NET 9.0 SDK
- Node.js
- npm

### Running the Project

1. Clone the repository
2. Navigate to the project directory
3. Restore .NET dependencies:
   ```bash
   dotnet restore
   ```
4. Navigate to the ClientApp directory and install npm packages:
   ```bash
   cd ClientApp
   npm install
   ```
5. Return to the root directory and run the project:
   ```bash
   dotnet run
   ```

The application will be available at `https://localhost:44363`

## 📝 Technical Implementation

The chess engine is built using modern bitboard techniques for efficient board representation and move generation. Here's a detailed overview of the implementation:

### Bitboard Architecture
- Uses 64-bit integers (`ulong`) to represent the chess board
- Each bit represents a square on the board
- Implements core bitwise operations (AND, OR, XOR, NOT)
- Efficient bit manipulation for piece movement and position analysis

### Piece Representation
- Maintains 12 separate bitboards:
  - 6 for white pieces (pawn, knight, bishop, rook, queen, king)
  - 6 for black pieces (pawn, knight, bishop, rook, queen, king)
- Each piece type implements its own move generation logic
- Special handling for unique piece movements and rules

### Advanced Move Generation
- **Magic Bitboards**: Optimized move generation for sliding pieces
  - Pre-computed attack tables
  - Perfect hashing through magic number multiplication
  - Separate tables for bishops and rooks
  - Optimized bit count for each square
- **Specialized Piece Logic**:
  - Pawns: Handles en passant and promotions
  - Knights: Uses pre-calculated attack patterns
  - Sliding pieces: Utilizes magic bitboard lookup
  - King: Includes castling logic

### Performance Optimizations
- O(1) sliding piece move generation
- Pre-computed attack tables
- Efficient bit manipulation operations
- File masks to prevent wrap-around moves
- Attack map caching for legal move validation

### Board Analysis
- Maintains attack maps for all pieces
- Efficient check detection
- Legal move filtering
- Position evaluation support
- Piece mobility calculation

This implementation follows best practices from modern chess engines, prioritizing performance while maintaining clean, object-oriented design principles.

## 📝 License

[MIT License](LICENSE) 
