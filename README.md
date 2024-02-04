# Campus-bulletin-board

## Introduction

The Campus Bulletin App serves as a sophisticated communication platform for universities. It empowers students, faculty, and staff by offering essential features such as channels, notices, and user management. Users can create channels, share notices, and subscribe to channels of interest. Real-time notifications significantly enhance the overall user experience, ensuring timely access to vital information.

## Architecture

The project adopts a microservices architecture, strategically chosen to achieve scalability, maintainability, and modularity. Microservices dismantle the monolithic structure into smaller, independent services, each handling specific functionalities. This architectural choice enhances the system's overall flexibility and resilience.

### Microservices Description

#### 1. Board.User Microservice

- Purpose: The User microservice takes charge of user-related operations, encompassing registration, authentication, and profile management. It guarantees a secure and personalized experience for each user.

#### 2. Board.Channel Microservice

- Purpose: The Channel microservice is dedicated to operations involving channels. It manages the creation, updates, and membership aspects, fostering organized and categorized communication within the academic community.

#### 3. Board.Notice Microservice

- Purpose: The Notice microservice assumes responsibility for the management of notices posted within channels. It oversees the creation, retrieval, and deletion of notices, ensuring the efficient dissemination of information.

#### 4. Board.Notification Microservice

- Purpose: The Notification microservice is a pivotal component in real-time communication. It handles the distribution of notifications to all members of a channel, ensuring prompt updates and active engagement.

## Communication

The microservices employ a hybrid communication approach, combining synchronous (HTTP requests) and asynchronous (RabbitMQ message broker) methods. Websockets, powered by SignalR, facilitate real-time communication for notices. This architectural blend ensures efficient communication, flexibility, and responsiveness, enhancing theeffectiveness of the Campus Bulletin App.
