import { Route, Routes } from 'react-router';
import Layout from './components/Layout';
import AddExistingPage from './pages/AddExistingPage';
import DashboardPage from './pages/DashboardPage';
import MasterlistPage from './pages/MasterlistPage';
import ComingSoonPage from './pages/ComingSoonPage';



function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route index element={<DashboardPage />}/>
        <Route path='masterlist' element={<MasterlistPage />}/>
        <Route path='create-document' element={<ComingSoonPage title="Create Document"/>}/>
        <Route path='revision' element={<ComingSoonPage title="Revision"/>}/>        
        <Route path='add-existing' element={<AddExistingPage />}/>
        <Route path='manage-users' element={<ComingSoonPage title="Manage Users"/>}/>
      </Route>
    </Routes>
  );
}

export default App;
