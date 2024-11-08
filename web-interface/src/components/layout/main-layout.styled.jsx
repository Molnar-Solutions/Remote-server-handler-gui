import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';

export const MainLayoutBox = styled(Box)(({theme}) => ({
    display: 'flex',
    width: "100%",
    background: "#10163A",
    [theme.breakpoints.up("lg")]: {
      flexDirection: "row"
    },
    [theme.breakpoints.down("md")]: {
      flexDirection: "column"
    },
}));