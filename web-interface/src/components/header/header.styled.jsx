import { styled } from '@mui/material/styles';
import { Grid, List, ListItem, SwipeableDrawer } from '@mui/material';
import IconButton from '@mui/material/IconButton';

const HEADER_MAX_WIDTH = 400;
const HEADER_MAX_HEIGHT = 200;

export const HeaderGrid = styled(Grid)(({ theme }) => ({
  width: "100%",
  [theme.breakpoints.up('lg')]: {
    maxWidth: HEADER_MAX_WIDTH,
  },
  [theme.breakpoints.down('lg')]: {
    maxWidth: "100%",
    height: HEADER_MAX_HEIGHT/2,
    flexDirection: "row",
    justifyContent: "center",
    gap: 5
  },
}));

export const HeaderLogo = styled('img')(({theme}) => ({
  maxWidth: "100px",
  width: "100%",
  height: "auto",
  objectFit: "scale-down",
}));

export const HeaderMainNavigation = styled(List)(({theme}) => ({
}));

export const ToggleBtn = styled(IconButton)(({ theme }) => ({
  visibility: 'initial',
  color: '#fff',
  ['.icon']: {
    width: 35,
    height: 35
  },
  [theme.breakpoints.up('lg')]: {
    display: 'none'
  }
}))

export const HeaderDrawer = styled(SwipeableDrawer)(({theme}) => ({
  ['.MuiPaper-root']: {
    color: "#fff",
    backgroundColor: '#10163A',
    width: "100%"
  },
}))

export const MyListItem = styled(ListItem)(({theme}) => ({
  maxWidth: 400,
  width: "100%",
  background: "#fff",
  border: "1px solid #777",
  borderRadius: 5,
  margin: "0.5rem 0"
}));