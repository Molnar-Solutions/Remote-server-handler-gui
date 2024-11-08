import "./header.styles.css";

import Typography from '@mui/material/Typography';
import * as React from 'react';
import {Fragment, useState} from 'react';
import {
    Avatar, Drawer,
    Grid,
    List,
    ListItemButton,
    ListItemIcon,
    ListItemText, Stack, Toolbar, useMediaQuery, useTheme,
} from '@mui/material';
import {Link, useNavigate} from 'react-router-dom';
import Logo from '../../assets/computer-156951_640.png';
import {HeaderDrawer, HeaderGrid, HeaderLogo, HeaderMainNavigation, MyListItem, ToggleBtn} from './header.styled';
import MenuIcon from "@mui/icons-material/Menu";
import {useSelector} from "react-redux";
import {getCurrentUser} from "../../store/reducers/user/user.reducer";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import {menus} from "../layout/main-layout.component";

export function HeaderItem({key, title, url, icon}) {
    const navigate = useNavigate();

    return (
        <MyListItem key={key}>
            <ListItemButton
                onClick={() => {
                    navigate(url);
                }}
            >
                { /* This is element's icon */}
                <ListItemIcon sx={{
                    color: "#202124"
                }}>{icon}</ListItemIcon>

                { /* This is element's text */}
                <ListItemText primary={title}/>
            </ListItemButton>
        </MyListItem>
    );
}

function SideMenu({isOpen, handleClose}) {
    const currentUser = useSelector(getCurrentUser);

    const drawerWidth = 340;

    return <Drawer
        open={isOpen}
        onClose={handleClose}
        sx={{
            width: drawerWidth,
            flexShrink: 0,
            "& .MuiDrawer-paper": {
                width: drawerWidth,
                boxSizing: "border-box",
                padding: "1rem 2rem",
            }
        }}
    >
        <Stack sx={{
            width: "100%"
        }} justifyContent={"flex-end"} alignItems={"flex-end"}>
            <IconButton onClick={handleClose} sx={{
                width: 40,
                height: 40
            }}>
                <CloseIcon/>
            </IconButton>
        </Stack>

        <Link to={"/"}>
            <HeaderLogo src={Logo} alt={'Remote Server Handler '}/>
        </Link>

        <Link to={"/"} style={{color: "initial", textDecoration: "none"}}>
            <Typography
                variant="h4"
                component="div"
                sx={{width: "100%", height: "auto", textAlign: "center", wordWrap: "wrap"}}
            >
                Remote Server Handler 
            </Typography>
            <br/>
        </Link>

        <HeaderMainNavigation>
            {
                menus
                    .map(({title, url, icon}, i) => {
                        return <HeaderItem key={title + " " + i} title={title} url={url} icon={icon}/>
                    })
            }
        </HeaderMainNavigation>
    </Drawer>
}

export default function Header({menus}) {
    const navigate = useNavigate();
    const theme = useTheme();

    const [open, setOpen] = useState(false);

    /* For responsive purposes */
    const underMedium = useMediaQuery(theme.breakpoints.down('md'));

    return <Stack
        flexDirection={"column"}
        justifyContent="flex-start"
        alignItems="center"
        sx={{
            backgroundColor: "#fff",
            color: "#171922",
            padding: underMedium ? "1rem 0.5rem 0.25rem 0.5rem" : "1.5rem",
            maxWidth: underMedium ? "100%" : "300px"
        }}
    >
        <Link to={"/"}>
            <HeaderLogo src={Logo} alt={'Remote Server Handler'}/>
        </Link>

        <Link to={"/"} style={{color: "initial", textDecoration: "none"}}>
            <Typography
                variant="h4"
                component="div"
                sx={{width: "100%", height: "auto", textAlign: "center"}}
            >
                Remote Server Handler
            </Typography>
            <br/>
        </Link>

        <HeaderMainNavigation>
            {
                !underMedium && menus.map(({title, url, icon}, i) => {
                    return <HeaderItem key={title + " " + i} title={title} url={url} icon={icon}/>
                })
            }
        </HeaderMainNavigation>

        {
            underMedium && <Fragment>
                <Stack flexDirection={"row"} justifyContent={underMedium ? "center" : "flex-end"} alignItems={"center"}>
                    <Toolbar>
                        <Stack flexDirection={"row"} justifyContent={"flex-end"} alignItems={"center"} gap={2}
                               className={"nav-item"}>
                            <Avatar
                                onClick={() => setOpen(!open)}
                                sx={{
                                    background: "#202124",
                                    color: "#fff"
                                }}>
                                <MenuIcon/>
                            </Avatar>
                        </Stack>
                    </Toolbar>
                </Stack>
            </Fragment>
        }

        <SideMenu isOpen={open} handleClose={() => setOpen(!open)} />
    </Stack>
}