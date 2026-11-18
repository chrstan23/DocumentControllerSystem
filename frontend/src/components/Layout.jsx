import { Outlet } from 'react-router';
import TabBar from './TabBar';

function Layout () {
    return (
        <div>
            <h1>Document Control System</h1>
            <TabBar />
            <hr />
            <Outlet />
        </div>
    );
}

export default Layout;