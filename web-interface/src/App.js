import './App.css';
import {useEffect} from "react";
import RequiredAuth from "./security/required-auth";
import MainLayout from "./components/layout/main-layout.component";
import {BrowserRouter, Route, Routes, useNavigate} from "react-router-dom";
import SignIn from "./routes/signin/sign-in.route";
import HomePage from "./routes/home/home.route";
import SystemHealth from "./routes/systemhealth/system-health.route";
import FileManager from "./routes/filemanager/file-manager.route";
import SignOut from "./routes/signout/sign-out.route";

function App() {
  return (
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<RequiredAuth />}>
            <Route element={<MainLayout />}>
              <Route index element={<HomePage />}/>
              <Route path="filemanager">
                <Route index element={<FileManager />}/>
              </Route>
              <Route path="systemhealth">
                <Route index element={<SystemHealth />}/>
              </Route>
            </Route>
          </Route>
          <Route path="/sign-in" element={<SignIn/>}/>
          <Route path="/sign-out" element={<SignOut/>}/>
          <Route path="*" element={<NavigateToHome/>}/>
        </Routes>
      </BrowserRouter>
  )
}


function NavigateToHome() {
  const navigate = useNavigate();

  useEffect(() => {
    navigate("/");
  }, []);

  return <div>...</div>;
}


export default App;
