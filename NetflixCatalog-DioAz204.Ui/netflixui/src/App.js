import React, { useState, useEffect } from 'react';
import axios from './api/NetflixApi';
import './App.css';
import AddMovieModal from './components/AddMovieModal';

function App() {
  const [movies, setMovies] = useState([]);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    fetchMovies();
  }, []);

  const fetchMovies = async () => {
    const response = await axios.get('api/GetAllMovies');
    setMovies(response.data);
  };

  const handleAddMovie = async (movieData) => {
    const formData = new FormData();
    formData.append('file', movieData.file);
    formData.append('movieData', JSON.stringify({
      name: movieData.name,
      description: movieData.description,
      category: movieData.category,
    }));

    try {
      await axios.post(`api/Movie`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      fetchMovies(); // Refresh the movie list
    } catch (error) {
      console.error('Error adding movie:', error);
    }
  };

  return (
    <div className="container mt-5">
      <h1 className="text-center mb-4">Bley Movies</h1>

      {/* Button to Open Modal */}
      <div className="text-center mb-4">
        <button class="btn btn-primary" onClick={() => setShowModal(true)}>
          Add New Movie
        </button>
      </div>

      {/* Add Movie Modal */}
      <AddMovieModal
        show={showModal}
        handleClose={() => setShowModal(false)}
        handleSubmit={handleAddMovie}
      />


      {/* Movie List */}
      <div className="row">
        {movies.map((movie) => (
          <div key={movie.id} className="col-md-4 mb-4">
            <div className="card h-100">
              <img
                src={movie.imageUrl}
                className="card-img-top"
                alt={movie.name}
                style={{ height: '300px', objectFit: 'cover' }}
              />
              <div className="card-body">
                <h5 className="card-title">{movie.name}</h5>
                <p className="card-text">{movie.description}</p>
                <p className="card-text">
                  <small className="text-muted">{movie.category}</small>
                </p>
              </div>
              <div className="card-footer">
                <small className="text-muted">
                  Added on: {new Date(movie.createdAt).toLocaleDateString()}
                </small>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default App;