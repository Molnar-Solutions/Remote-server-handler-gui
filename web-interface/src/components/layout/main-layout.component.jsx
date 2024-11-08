import './main-layout.styles.css';

import * as React from 'react';
import Box from '@mui/material/Box';
import {Link, Outlet, useNavigate} from 'react-router-dom';
import Header, {HeaderItem} from '../header/header.component';
import {MainLayoutBox} from './main-layout.styled';
import HomeIcon from "@mui/icons-material/Home";
import PetsIcon from "@mui/icons-material/Pets";
import LogoutTwoToneIcon from "@mui/icons-material/LogoutTwoTone";

import PostAddIcon from '@mui/icons-material/PostAdd';
import AddToDriveIcon from '@mui/icons-material/AddToDrive';
import HealthAndSafetyIcon from '@mui/icons-material/HealthAndSafety';

export const menus = [
    {
        title: "Home",
        url: '/',
        icon: <HomeIcon/>
    },
    {
        title: "File manager",
        url: '/filemanager',
        icon: <AddToDriveIcon/>
    },
    {
        title: "System health",
        url: '/systemhealth',
        icon: <HealthAndSafetyIcon/>
    },
    {
        title: "Sign out",
        url: '/sign-out',
        icon: <LogoutTwoToneIcon/>
    }
]

export default function MainLayout() {
    return (
        <MainLayoutBox>
            <Header menus={menus}/>

            <Box component='main' sx={{overflow: "scroll", height: "100vh"}} className='main'>
                <Outlet/>
            </Box>
        </MainLayoutBox>
    );
}